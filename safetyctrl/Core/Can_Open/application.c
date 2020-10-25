/*
 * Application interface for CANopenNode stack.
 *
 * @file        application.c
 * @ingroup     application
 * @author      Janez Paternoster
 * @copyright   2012 - 2013 Janez Paternoster
 *
 * This file is part of CANopenNode, an opensource CANopen Stack.
 * Project home page is <https://github.com/CANopenNode/CANopenNode>.
 * For more information on CANopen see <http://www.can-cia.org/>.
 *
 * CANopenNode is free and open source software: you can redistribute
 * it and/or modify it under the terms of the GNU General Public License
 * as published by the Free Software Foundation, either version 2 of the
 * License, or (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program. If not, see <http://www.gnu.org/licenses/>.
 *
 * Following clarification and special exception to the GNU General Public
 * License is included to the distribution terms of CANopenNode:
 *
 * Linking this library statically or dynamically with other modules is
 * making a combined work based on this library. Thus, the terms and
 * conditions of the GNU General Public License cover the whole combination.
 *
 * As a special exception, the copyright holders of this library give
 * you permission to link this library with independent modules to
 * produce an executable, regardless of the license terms of these
 * independent modules, and to copy and distribute the resulting
 * executable under terms of your choice, provided that you also meet,
 * for each linked independent module, the terms and conditions of the
 * license of that module. An independent module is a module which is
 * not derived from or based on this library. If you modify this
 * library, you may extend this exception to your version of the
 * library, but you are not obliged to do so. If you do not wish
 * to do so, delete this exception statement from your version.
 */


#include "CANopen.h"
#include "xprintf.h"
#include "Defines.h"


/*  Send a SDO with a given value
	The Reciver is defined Function SDO Communcation */
void Send_SDO(uint8_t value) {

	//SDO-Data that will be send
	uint8_t data[] = { value,0x00 };
	uint32_t retClientDownload = 0;

	//Send message to reciver
	if (CO_SDOclientDownloadInitiate(
		CO->SDOclient,//CO_SDOclient_t	*SDO_C,
		0x6040,	//uint16_t				index,
		0x00,	//uint8_t				subIndex,
		data,	//uint8_t				*dataTx,
		0x0002,	//uint32_t				dataSize,
		0x00)	//uint8_t				blockEnable)
	  != CO_SDOcli_ok_communicationEnd) {
		xprintf("CO_SDOclientDownloadInitiate failed.\n");
		while (1);
	}

	HAL_Delay(1);

	//Debug
	// wait for the answer from the reciver
	while (0 != CO_SDOclientDownload(
		CO->SDOclient,//CO_SDOclient_t		*SDO_C,
		0x000A,	//uint16_t					timeDifference_ms,
		0x00FF,	//uint16_t					SDOtimeoutTime,
		&retClientDownload)) {//uint32_t	*pSDOabortCode)
		xprintf("Waiting for Server Response\n");
	}

	//possibility to print the reached SDO message
	/*
	xprintf("SDODownload: \n");
	for(i=0;i<8;i++)
		xprintf("SDOD[%d]: %d\t",i,CO->SDOclient->CANrxData[i]);
	*/
}

/*	Check if all motors are allive
	Send every motor a SDO request to send the statusword and check if they send the statusword back*/
void motorDetection(void) {
	int i = 0;
	uint8_t ID = 0;

	// 0: motor not answered
	// 1: motor answerd
	char answered[8] = { 0 };

	//Loop over all motors until everyone send a satusword back
	while (MotorsAreNotAlive)
	{
		for (i = 1; i <= 8; i++)
		{
			if (!answered[i - 1])
			{

				uint8_t dataRx[0xFF] = { 0 };
				uint32_t DataSize = 0;
				uint32_t retClientDownload = 0;

				switch (i)
				{
				case 1:
					ID = MotorIDGoalieRotation;
					break;
				case 2:
					ID = MotorIDDefenseRotation;
					break;
				case 3:
					ID = MotorIDMidfieldRotation;
					break;
				case 4:
					ID = MotorIDOffenseRotation;
					break;
				case 5:
					ID = MotorIDGoalieMove;
					break;
				case 6:
					ID = MotorIDDefenseMove;
					break;
				case 7:
					ID = MotorIDMidfieldMove;
					break;
				case 8:
					ID = MotorIDOffenseMove;
					break;
				default:
					ID = 0x00;
					break;
				}

				//start communcation with the motor
				Start_SDO_Communication(ID);

				//send SDO for statusword request
				if (CO_SDOclientUploadInitiate(
					CO->SDOclient,//CO_SDOclient_t	*SDO_C,
					0x6041,	//uint16_t              index,
					0x00,	//uint8_t				subIndex,
					dataRx,	//uint8_t               *dataRx,
					0x00FF,	//uint32_t				dataRxSize,
					0x00)	//uint8_t               blockEnable
					!= CO_SDOcli_ok_communicationEnd) {
					xprintf("ClientUploadInitiate Error.\n");
					while (1);
				}

				HAL_Delay(1);

				// check if the motor have answered
				if (CO_SDOcli_ok_communicationEnd == CO_SDOclientUpload(
					CO->SDOclient,	//CO_SDOclient_t	*SDO_C,
					0x000A, //uint16_t					timeDifference_ms,
					0x00FF, //uint16_t					SDOtimeoutTime,
					&DataSize,//uint32_t				*pDataSize,
					&retClientDownload)) {//uint32_t    *pSDOabortCode)
						answered[i - 1] = 1;
				}
			}
		}

		//Debug
		//posibillity to print out which motors allready have answerd
		/*
		int a = 0;
		xprintf("State: \n");
		for (a = 0; a < 8; a++)
			xprintf("%d - 0x%x \t", a, answered[a]);
		xprintf("\n");
		for (a = 0; a < 8; a++)
			xprintf("SDOD[%d]: 0x%x\t", i, CO->SDOclient->CANrxData[i]);
		xprintf("\n");
		*/
	}
}


// send a SDO with request to send back a statusword to motor
// wait for the statusword and give it back
/*
uint16_t Read_SDO() {

	uint8_t dataRx[0xFF] = { 0 };
	uint32_t DataSize = 0;
	uint32_t retClientDownload = 0;
	//to save the return value
	uint16_t statusword = 0;
	int i = 0;

	// send SDO for request
	if (CO_SDOclientUploadInitiate(
		CO->SDOclient,//CO_SDOclient_t	*SDO_C,
		0x6041,	//uint16_t				index,
		0x00,	//uint8_t				subIndex,
		dataRx,	//uint8_t				*dataRx,
		0x00FF,	//uint32_t				dataRxSize,
		0x00)	//uint8_t				blockEnable
		!= CO_SDOcli_ok_communicationEnd) {
		xprintf("ClientUploadInitiate Error.\n");
		while (1);
	}

	HAL_Delay(1);

	// wait for answer
	while (0 != CO_SDOclientUpload(
		CO->SDOclient,//CO_SDOclient_t		*SDO_C,
		0x000A,		//uint16_t              timeDifference_ms,
		0x00FF,		//uint16_t              SDOtimeoutTime,
		&DataSize,	//uint32_t              *pDataSize,
		&retClientDownload)) {//uint32_t	*pSDOabortCode);){
		xprintf("Waiting for Response..\n");
	}

	HAL_Delay(1);

	//Debug
	// posibillity to give out the recieved answer
	xprintf("CommReset finished, retClientDownload: %d\n", retClientDownload);
	xprintf("Datasize= %d\nSDOUpload:\n", DataSize);
	for (i = 0; i < 8; i++)
		//xprintf("SDO Daten[%d]: %d\t",i,dataRx[i]);
		xprintf("SDOD[%d]: 0x%x\t", i, CO->SDOclient->CANrxData[i]);
	xprintf("\n");
	

	statusword |= CO->SDOclient->CANrxData[7];
	statusword <<= 4;
	statusword |= CO->SDOclient->CANrxData[6];
	statusword <<= 4;
	statusword |= CO->SDOclient->CANrxData[5];
	statusword <<= 4;
	statusword |= CO->SDOclient->CANrxData[4];


	return statusword;
}
*/

// Start the SDO communcation with a Motor
// must only be called then the reciver for the SDO change
// then sending more SDO to one Motor it must only be called once
void Start_SDO_Communication(uint8_t motorNumber) {
	if (CO_SDOclient_setup(
		CO->SDOclient,			//CO_SDOclient_t	*SDO_C,
		0x600 | motorNumber,	//uint32_t			COB_IDClientToServer,
		0x580 | motorNumber,	//uint32_t          COB_IDServerToClient,
		0x00  | motorNumber)	//uint8_t           nodeIDOfTheSDOServer);
		!= CO_SDOcli_ok_communicationEnd) {
		xprintf("SDOclient_setup error.\n");
		while (1);
	}
}


// bring motor in NMT-Stop 
void Stop_Motor(uint8_t motorNumber) {
	//1. Start SDO Communication with Node
	//2. Send SDO for Shutdwon
	//3. Send NMT-Stop
	Start_SDO_Communication(motorNumber);
	Send_SDO(Shutdown);
	CO_sendNMTcommand(CO, CO_NMT_ENTER_STOPPED, motorNumber);
}

// initialise motor at the beginning or after a NMT-Stop
void init_motor(uint8_t motorNumber) {

	//Prepare communication
	Start_SDO_Communication(motorNumber);
	//Send the needed controlwords for the states
	Send_SDO(DisableVoltage);
	Send_SDO(Shutdown);
	Send_SDO(SwitchOn);
	Send_SDO(EnableOperation);
}


//switch all motors and the can card to NMT-Stop
void Stop_Nodes(void) {

	//Send Can-Card in NMT-Stop
	CO_sendNMTcommand(CO, CO_NMT_ENTER_STOPPED, 0x15);

	//Send motor 1 - 4 in NMT-Stop (Faulhaber)
	Stop_Motor(MotorIDGoalieRotation);
	Stop_Motor(MotorIDDefenseRotation);
	Stop_Motor(MotorIDMidfieldRotation);
	Stop_Motor(MotorIDOffenseRotation);

	//Send Motor B - E in NMT-Stop (Tele)
	Stop_Motor(MotorIDGoalieMove);
	Stop_Motor(MotorIDDefenseMove);
	Stop_Motor(MotorIDMidfieldMove);
	Stop_Motor(MotorIDOffenseMove);
}

// bring all motors to the motor operation mode "run"
// it been called at the start for initalization 
// and after NMT-Stop
void communicationReset(void) {
	CO_sendNMTcommand(CO, CO_NMT_ENTER_PRE_OPERATIONAL, 0);
	init_motor(MotorIDGoalieRotation);
	HAL_Delay(1);
	init_motor(MotorIDDefenseRotation);
	HAL_Delay(1);
	init_motor(MotorIDMidfieldRotation);
	HAL_Delay(1);
	init_motor(MotorIDOffenseRotation);
	HAL_Delay(1);
	init_motor(MotorIDGoalieMove);
	HAL_Delay(1);
	init_motor(MotorIDDefenseMove);
	HAL_Delay(1);
	init_motor(MotorIDMidfieldMove);
	HAL_Delay(1);
	init_motor(MotorIDOffenseMove);
	HAL_Delay(1);
	CO_sendNMTcommand(CO, CO_NMT_ENTER_OPERATIONAL, 0);
}

/*******************************************************************************/
void programStart(void) {
	//xprintf("programmStartup.\n");
}


/*******************************************************************************/
void programEnd(void) {
	//xprintf("programmEnd.\n");
}


/*******************************************************************************/
void programAsync(uint16_t timer1msDiff) {
	//xprintf("programmAsync.\n");
}


/*******************************************************************************/
void program1ms(void) {
	//xprintf("programm1ms.\n");
}
