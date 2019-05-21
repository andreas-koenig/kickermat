namespace ObjectSearch.BlobSearch
{
    using System.Drawing;
    using System.Windows.Forms;
    using Emgu.CV;
    using Emgu.CV.Structure;
    using GlobalDataTypes;
    using ObjectSearch.Base;
    using Emgu.CV.Cvb;
    using System.Collections.Generic;
    using System.Linq;
    using System;
    using System.Threading;

    /// <summary>
    /// Basisklasse für alle Objekterkennungsinstanzen
    /// </summary>
    public sealed class BlobSearch : BasicObjectSearch<BlobSearchSettings>
    {
        /// <summary>
        /// Gets the BLOB count.
        /// </summary>
        /// <value>The BLOB count.</value>
        public override int NumberOfFoundObjects
        {
            get
            {
                return _Blobs != null ? _Blobs.Count : 0;
            }
        }
        public override int GetBiggestObjectIndex()
        {
            int i = 0, index = 0, MaxArea = -1; ;
            foreach (var blob in this._Blobs)
            {
                if (blob.BlobSize > MaxArea)
                {
                    MaxArea = blob.BlobSize;
                    index = i;
                }
                i++;
            }
            return index;
        }
        private List<BlobData> _Blobs = null;
        private ulong _LatestFrameCount = 0;
        /// <summary>
        /// Gets the child user control.
        /// </summary>
        /// <value>The child user control.</value>
        protected override UserControl ChildUserControl
        {
            get { return new BlobSearchUserControl(this.Settings); }
        }

        /// <summary>
        /// Retunrs the size of a found blob.
        /// </summary>
        /// <param name="index">The index of the blob.</param>
        /// <returns>The size of the blob.</returns>
        public override int ObjectSize(int index)
        {
            if (_Blobs == null)
                return -1;
            return (index >= 0 && index < _Blobs.Count) ? _Blobs[index].BlobSize : -1;
        }

        /// <summary>
        /// Sorts the found blobs.
        /// </summary>
        public override void SortObjects()
        {
        }

        /// <summary>
        /// Gets the object center.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns>The object center.</returns>
        public override Position GetObjectCenter(int index)
        {
            if (_Blobs == null || index < 0 || index >= _Blobs.Count)
                return null;
            return new Position(_Blobs[index].BlobCenterX, _Blobs[index].BlobCenterY, true, true) { BoundingBox = _Blobs[index].Rectangle };
        }

        /// <summary>
        /// Gets the bounding box of an object.
        /// </summary>
        /// <param name="index">The index of the object.</param>
        /// <returns>The bounding box of the object.</returns>
        public override Rectangle GetObjectBounds(int index)
        {
            if (_Blobs == null)
                return new Rectangle();
            return (index >= 0 && index < _Blobs.Count) ? _Blobs[index].Rectangle : Rectangle.Empty;
        }

        /// <summary>
        /// Finds the blobs.
        /// </summary>
        /// <param name="binarizedImage">The image for detection.</param>
        protected override void FindObjects(IImage binarizedImage, ulong framecount)
        {
            Image<Gray, byte> gray_image;
            if (binarizedImage is Image<Gray, byte>)
                gray_image = binarizedImage as Image<Gray, byte>;
            else
                gray_image = new Image<Gray, byte>(binarizedImage.Bitmap);
            var detector = new CvBlobDetector();
            var blobs = new CvBlobs();
            List<BlobData> newBlobdata = new List<BlobData>();
            //First Search in Area Of Interest
            gray_image.ROI = AreaOfInterestForNextSearch;
            detector.Detect(gray_image, blobs);
            blobs.FilterByArea(this.Settings.MinBlobArea, this.Settings.MaxBlobArea);
            var blobList = filterBlobsByDistanceToEachOther(blobs.Values.ToList());
            //if we are already behind return;
            if (framecount < _LatestFrameCount)
                return;
            if (blobList.Count >= Settings.MinimumNumberOfObjectsToFind)
            {
                foreach (var item in blobList)
                {
                    var blobData = new BlobData(
                        (int)item.Centroid.X + gray_image.ROI.X,
                        (int)item.Centroid.Y + gray_image.ROI.Y,
                        item.Area,
                         new Rectangle(item.BoundingBox.X + gray_image.ROI.X, item.BoundingBox.Y + gray_image.ROI.Y, item.BoundingBox.Width, item.BoundingBox.Height)
                        );
                    newBlobdata.Add(blobData);
                }
                SetBlobsAndAreaOfInterest(newBlobdata, getNextAreaOfInterest(newBlobdata), framecount);
                return;
            }
            //if we are already behind return;
            if (framecount < _LatestFrameCount)
                return;
            //If no Blobs have been found in Are of Interest search in the Playing field area
            gray_image.ROI = PLayingFieldArea;
            detector.Detect(gray_image, blobs);
            blobs.FilterByArea(this.Settings.MinBlobArea, this.Settings.MaxBlobArea);
            blobList = filterBlobsByDistanceToEachOther(blobs.Values.ToList());
            //if we are already behind return;
            if (framecount < _LatestFrameCount)
                return;
            if (blobList.Count >= Settings.MinimumNumberOfObjectsToFind)
            {
                foreach (var item in blobList)
                {
                    var blobData = new BlobData(
                        (int)item.Centroid.X + gray_image.ROI.X,
                        (int)item.Centroid.Y + gray_image.ROI.Y,
                        item.Area,
                         new Rectangle(item.BoundingBox.X + gray_image.ROI.X, item.BoundingBox.Y + gray_image.ROI.Y, item.BoundingBox.Width, item.BoundingBox.Height)
                        );
                    newBlobdata.Add(blobData);
                }
                SetBlobsAndAreaOfInterest(newBlobdata, getNextAreaOfInterest(newBlobdata), framecount);
                return;
            }
            //if we are already behind return;
            if (framecount < _LatestFrameCount)
                return;
            //If still No Blobs have been found search the Entire Image;
            gray_image.ROI = Rectangle.Empty;
            detector.Detect(gray_image, blobs);
            blobs.FilterByArea(this.Settings.MinBlobArea, this.Settings.MaxBlobArea);
            blobList = filterBlobsByDistanceToEachOther(blobs.Values.ToList());
            //if we are already behind return;
            if (framecount < _LatestFrameCount)
                return;
            foreach (var item in blobList)
            {
                var blobData = new BlobData(
                    (int)item.Centroid.X + gray_image.ROI.X,
                    (int)item.Centroid.Y + gray_image.ROI.Y,
                    item.Area,
                     new Rectangle(item.BoundingBox.X + gray_image.ROI.X, item.BoundingBox.Y + gray_image.ROI.Y, item.BoundingBox.Width, item.BoundingBox.Height)
                    );
                newBlobdata.Add(blobData);
            }
            SetBlobsAndAreaOfInterest(newBlobdata, Rectangle.Empty, framecount);
        }

        private void SetBlobsAndAreaOfInterest(List<BlobData> blobs, Rectangle areaOfInterest, ulong framecount)
        {
            if (framecount > _LatestFrameCount)
            {
                try
                {
                    _Lock.AcquireWriterLock(10);
                    //Threadsafe Area
                    {
                        _LatestFrameCount = framecount;
                        _Blobs = blobs;
                        AreaOfInterestForNextSearch = areaOfInterest;
                    }
                }
                finally
                {
                    _Lock.ReleaseWriterLock();
                }
            }
        }

        private readonly ReaderWriterLock _Lock = new ReaderWriterLock();

        private Rectangle getNextAreaOfInterest(List<BlobData> blobs)
        {
            if (blobs.Count == 1)
            {
                var blob = blobs[0];
                return new Rectangle((int)blob.BlobCenterX - Settings.AreaOfInterestWidth / 2, (int)blob.BlobCenterY - Settings.AreaOfInterestHeight / 2, Settings.AreaOfInterestWidth, Settings.AreaOfInterestHeight);
            }
            else if (blobs.Count > 1)
            {
                int xMax = Int32.MinValue, yMax = Int32.MinValue, xMin = Int32.MaxValue, yMin = Int32.MaxValue;
                foreach (var item in blobs)
                {
                    if ((int)item.BlobCenterX > xMax)
                        xMax = (int)item.BlobCenterX;
                    if ((int)item.BlobCenterY > yMax)
                        yMax = (int)item.BlobCenterY;
                    if ((int)item.BlobCenterX < xMin)
                        xMin = (int)item.BlobCenterX;
                    if ((int)item.BlobCenterY < yMin)
                        yMin = (int)item.BlobCenterY;
                }
                return new Rectangle(xMin - Settings.AreaOfInterestWidth / 2, yMin - Settings.AreaOfInterestHeight / 2, xMax - xMin + Settings.AreaOfInterestWidth, yMax - yMin + Settings.AreaOfInterestHeight);
            }
            else
                return Rectangle.Empty;
        }

        private List<CvBlob> filterBlobsByDistanceToEachOther(IEnumerable<CvBlob> blobs)
        {
            return blobs.Where(p =>
            {
                foreach (var item in blobs)
                {
                    if (item != p)
                    {
                        bool xDistanceTooClose = false, yDistanceTooClose = false;
                        if (p.Centroid.X < item.Centroid.X)
                        {
                            //MindestAbstand in x nicht eingehalten
                            if (Math.Abs((p.Centroid.X + p.BoundingBox.Width / 2) - (item.Centroid.X - item.BoundingBox.Width / 2)) <= Settings.DistanceToSearchNextBlob)
                                xDistanceTooClose = true;
                        }
                        else
                        {
                            if (Math.Abs((p.Centroid.X - p.BoundingBox.Width / 2) - (item.Centroid.X + item.BoundingBox.Width / 2)) <= Settings.DistanceToSearchNextBlob)
                                xDistanceTooClose = true;
                        }
                        if (!xDistanceTooClose)
                            continue;
                        if (p.Centroid.Y < item.Centroid.Y)
                        {
                            if (Math.Abs((p.Centroid.Y + p.BoundingBox.Height / 2) - (item.Centroid.Y - item.BoundingBox.Height / 2)) <= Settings.DistanceToSearchNextBlob)
                                yDistanceTooClose = true;
                        }
                        else
                        {
                            if (Math.Abs((p.Centroid.Y - p.BoundingBox.Height / 2) - (item.Centroid.Y + item.BoundingBox.Height / 2)) <= Settings.DistanceToSearchNextBlob)
                                yDistanceTooClose = true;
                        }
                        //Too close in x 
                        if (xDistanceTooClose && yDistanceTooClose)
                        {
                            //if p smaller than item dont take it
                            if (p.Area < item.Area)
                                return false;
                        }
                    }
                }
                return true;
            }).ToList();
        }
    }
}