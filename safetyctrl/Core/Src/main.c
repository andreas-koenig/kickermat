/* USER CODE BEGIN Header */
/**
  ******************************************************************************
  * @file           : main.c
  * @brief          : Main program body
  ******************************************************************************
  * @attention
  *
  * <h2><center>&copy; Copyright (c) 2019 STMicroelectronics.
  * All rights reserved.</center></h2>
  *
  * This software component is licensed by ST under BSD 3-Clause license,
  * the "License"; You may not use this file except in compliance with the
  * License. You may obtain a copy of the License at:
  *                        opensource.org/licenses/BSD-3-Clause
  *
  ******************************************************************************
  */
/* USER CODE END Header */

/* Includes ------------------------------------------------------------------*/
#include "main.h"
#include "../Can_Open/CANopen.h"
#include "../Can_Open/application.h"

#define TMR_TASK_INTERVAL 	(1000)
#define INCREMENT_1MS(var)	(var++)

struct CANbase{
	uintptr_t baseAddress;
};

volatile uint16_t CO_timer1ms=0;

TIM_HandleTypeDef TimerHandle;
TIM_HandleTypeDef Timer3Handle;
CAN_HandleTypeDef hcan;
UART_HandleTypeDef huart2;

void SystemClock_Config(void);
static void MX_GPIO_Init(void);
static void MX_TIMER_init(void);
static void MX_TIMER3_init(void);
static void MX_USART2_UART_Init(void);


// For the uart communication with the pc
// possible with the arduino monitor (Baudrate: 115200)
void uart_putc(uint8_t d) {
    HAL_UART_Transmit(&huart2, &d, 1, 1000);
}

uint8_t uart_getc(void) {
    uint8_t d;
    (uint8_t)HAL_UART_Receive(&huart2, &d, 1, 1000);
    return d;
}



int main(void)
{
    //for the uart communication
    xdev_in(uart_getc);
    xdev_out(uart_putc);

    CO_NMT_reset_cmd_t reset = CO_RESET_NOT;

   
    HAL_Init();
    SystemClock_Config();
    MX_GPIO_Init();
    MX_USART2_UART_Init();

    OD_powerOnCounter++;

    programStart();

    //loop when CANopen Node is running
    while (reset != CO_RESET_APP) {
        /* CANopen communication reset - initialize CANopen objects *******************/
        CO_ReturnError_t err;
        uint16_t timer1msPrevious;

        struct CANbase canBase = {
            .baseAddress = 0u,  /* CAN module address */
        };

        /* initialize CANopen */
        err = CO_init((uint32_t)&hcan, 43/* NodeID */, 1000 /* bit rate */);
        if (err != CO_ERROR_NO) {
            while (1)
                xprintf("CO Init Error.\n");
        }

        /* Configure Timer interrupt function for execution every 1 millisecond */
        //light curtain
        MX_TIMER3_init();
        /* Configure Timer interrupt function for execution every 1 millisecond */
        //synchron task
        MX_TIMER_init();

        /* Configure CAN transmit and receive interrupt */

        /* start CAN */
        CO_CANsetNormalMode(CO->CANmodule[0]);

        reset = CO_RESET_NOT;
        timer1msPrevious = CO_timer1ms;

        // check if all motors are allive
        // function can be find in Can_Open/application.c
        motorDetection();


        // because of the Kickerkasten the voltage is delayed about 20 seconds
        // then the are not waiting the tele motors will get an error
        // the reason can be 
        HAL_Delay(30000);

        //initialisation of all motors
        // function can be find in Can_Open/application.c
        communicationReset();

        /* loop for normal program execution ******************************************/
        while (reset == CO_RESET_NOT) {

            
            uint16_t timer1msCopy, timer1msDiff;
            timer1msCopy = CO_timer1ms;
            timer1msDiff = timer1msCopy - timer1msPrevious;
            timer1msPrevious = timer1msCopy;

            // note in use
            // for thing that need some time
            programAsync(timer1msDiff);

            /* CANopen process */
            reset = CO_process(CO, timer1msDiff, NULL);

            /* Nonblocking application code may go here. */

            /* Process EEPROM */
        }
    }

    /* program exit ***************************************************************/
        /* stop threads */

    // not in use
    // then things must be done before can objects are deleted
    programEnd();
    /* delete objects from memory */
    CO_delete((void*)0/* CAN module address */);


    /* reset */
    return 0;
}

/* timer thread executes in constant intervals (1ms) ********************************/
static void tmrTask_thread(void) {

    for (;;) {

        /* sleep for interval */
        INCREMENT_1MS(CO_timer1ms);

        if (CO->CANmodule[0]->CANnormal) {
            bool_t syncWas;

            /* Process Sync and read inputs */
            syncWas = CO_process_SYNC_RPDO(CO, TMR_TASK_INTERVAL);

            /* Further I/O or nonblocking application code may go here. */

            /* Write outputs */
            CO_process_TPDO(CO, syncWas, TMR_TASK_INTERVAL);

            /* verify timer overflow */
            if (0) {
                CO_errorReport(CO->em, CO_EM_ISR_TIMER_OVERFLOW, CO_EMC_SOFTWARE_INTERNAL, 0U);
            }
        }
    }
}

bool Zustand = false;

//timer interrupt for light curtain
void TIM3_IRQHandler(void) {
    HAL_TIM_IRQHandler(&Timer3Handle);

    // PIN "Low": engange in light curtain (stop)
    // PIN "High": light curtain free
    GPIO_PinState PINC12 = HAL_GPIO_ReadPin(GPIOC, GPIO_PIN_12);

    if (PINC12 == GPIO_PIN_SET && !Zustand) {
        //Stop all CANopen Nodes
        // function can be find in Can_Open/application.c
        Stop_Nodes();
        xprintf("Rising Edge.\n");
        Zustand = true;
    }
    else if (PINC12 == GPIO_PIN_RESET && Zustand) {
        //Start all CANopen Nodes
        // function can be find in Can_Open/application.c
        communicationReset();
        xprintf("Falling Edge.\n");
        Zustand = false;
    }
}

void /* interrupt */ CO_CAN1InterruptHandler(void) {
    CO_CANinterrupt(CO->CANmodule[0]);
    /* clear interrupt flag */
}


void SystemClock_Config(void)
{
    RCC_OscInitTypeDef RCC_OscInitStruct = { 0 };
    RCC_ClkInitTypeDef RCC_ClkInitStruct = { 0 };

    /** Initializes the CPU, AHB and APB busses clocks
    */
    RCC_OscInitStruct.OscillatorType = RCC_OSCILLATORTYPE_HSE;
    RCC_OscInitStruct.HSEState = RCC_HSE_BYPASS;
    RCC_OscInitStruct.HSEPredivValue = RCC_HSE_PREDIV_DIV1;
    RCC_OscInitStruct.HSIState = RCC_HSI_ON;
    RCC_OscInitStruct.PLL.PLLState = RCC_PLL_ON;
    RCC_OscInitStruct.PLL.PLLSource = RCC_PLLSOURCE_HSE;
    RCC_OscInitStruct.PLL.PLLMUL = RCC_PLL_MUL9;
    if (HAL_RCC_OscConfig(&RCC_OscInitStruct) != HAL_OK)
    {
        Error_Handler();
    }
    /** Initializes the CPU, AHB and APB busses clocks
    */
    RCC_ClkInitStruct.ClockType = RCC_CLOCKTYPE_HCLK | RCC_CLOCKTYPE_SYSCLK
        | RCC_CLOCKTYPE_PCLK1 | RCC_CLOCKTYPE_PCLK2;
    RCC_ClkInitStruct.SYSCLKSource = RCC_SYSCLKSOURCE_PLLCLK;
    RCC_ClkInitStruct.AHBCLKDivider = RCC_SYSCLK_DIV2;//war _DIV1
    RCC_ClkInitStruct.APB1CLKDivider = RCC_HCLK_DIV1;//war _DIV2
    RCC_ClkInitStruct.APB2CLKDivider = RCC_HCLK_DIV1;//war _DIV1

    if (HAL_RCC_ClockConfig(&RCC_ClkInitStruct, FLASH_LATENCY_2) != HAL_OK)
    {
        Error_Handler();
    }
}

// Timer for synchron task
static void MX_TIMER_init(void) {

    __HAL_RCC_TIM2_CLK_ENABLE();

    TimerHandle.Instance = TIM2;
    TimerHandle.Init.CounterMode = TIM_COUNTERMODE_UP;
    TimerHandle.Init.Period = 29535;
    TimerHandle.Init.Prescaler = 2;
    TimerHandle.Init.ClockDivision = TIM_CLOCKDIVISION_DIV1;
    TimerHandle.Init.RepetitionCounter = 0;

    HAL_TIM_Base_Init(&TimerHandle);
    HAL_NVIC_SetPriority(TIM2_IRQn, 4, 1);
    HAL_NVIC_EnableIRQ(TIM2_IRQn);
    HAL_TIM_Base_Start_IT(&TimerHandle);
}

// Timer for the light curtain
static void MX_TIMER3_init(void) {

    __HAL_RCC_TIM3_CLK_ENABLE();
    Timer3Handle.Instance = TIM3;
    Timer3Handle.Init.CounterMode = TIM_COUNTERMODE_UP;
    Timer3Handle.Init.Period = 29535;
    Timer3Handle.Init.Prescaler = 2;
    Timer3Handle.Init.ClockDivision = TIM_CLOCKDIVISION_DIV1;
    Timer3Handle.Init.RepetitionCounter = 0;

    HAL_TIM_Base_Init(&Timer3Handle);
    HAL_NVIC_SetPriority(TIM3_IRQn, 3, 1);
    HAL_NVIC_EnableIRQ(TIM3_IRQn);
    HAL_TIM_Base_Start_IT(&Timer3Handle);
}


static void MX_USART2_UART_Init(void)
{
    huart2.Instance = USART2;
    huart2.Init.BaudRate = 115200;
    huart2.Init.WordLength = UART_WORDLENGTH_8B;
    huart2.Init.StopBits = UART_STOPBITS_1;
    huart2.Init.Parity = UART_PARITY_NONE;
    huart2.Init.Mode = UART_MODE_TX_RX;
    huart2.Init.HwFlowCtl = UART_HWCONTROL_NONE;
    huart2.Init.OverSampling = UART_OVERSAMPLING_16;
    if (HAL_UART_Init(&huart2) != HAL_OK)
    {
        Error_Handler();
    }
}


static void MX_GPIO_Init(void)
{
    GPIO_InitTypeDef GPIO_InitStruct = { 0 };

    /* GPIO Ports Clock Enable */

    __HAL_RCC_GPIOA_CLK_ENABLE();
    __HAL_RCC_GPIOB_CLK_ENABLE();
    __HAL_RCC_GPIOC_CLK_ENABLE();
    __HAL_RCC_GPIOD_CLK_ENABLE();

    /*Configure GPIO pin Output Level */
    HAL_GPIO_WritePin(GPIOB, GPIO_PIN_2, GPIO_PIN_RESET);

    /*Configure GPIO pin : PB2 */
    GPIO_InitStruct.Pin = GPIO_PIN_2;
    GPIO_InitStruct.Mode = GPIO_MODE_OUTPUT_PP;
    GPIO_InitStruct.Pull = GPIO_NOPULL;
    GPIO_InitStruct.Speed = GPIO_SPEED_FREQ_LOW;
    HAL_GPIO_Init(GPIOB, &GPIO_InitStruct);

    //warning: GPIO_InitStruct is reused, could cause complications

    /*Configure GPIO pin : PC12 to Detect Rising Edge */
    GPIO_InitStruct.Pin = GPIO_PIN_12;
    GPIO_InitStruct.Mode = GPIO_MODE_INPUT;
    GPIO_InitStruct.Pull = GPIO_NOPULL;
    GPIO_InitStruct.Speed = GPIO_SPEED_FREQ_HIGH;
    HAL_GPIO_Init(GPIOC, &GPIO_InitStruct);

    /*Configure GPIO pin: PD2 to Detect Falling Edge*/
    GPIO_InitStruct.Pin = GPIO_PIN_2;
    GPIO_InitStruct.Mode = GPIO_MODE_IT_FALLING;
    GPIO_InitStruct.Pull = GPIO_NOPULL;
    GPIO_InitStruct.Speed = GPIO_SPEED_FREQ_LOW;
    HAL_GPIO_Init(GPIOD, &GPIO_InitStruct);

}

// Timer for synchron task
void TIM2_IRQHandler(void) {
    HAL_TIM_IRQHandler(&TimerHandle);
}




//Synchroner Thread
void HAL_TIM_PeriodElapsedCallback(TIM_HandleTypeDef* htim) {
    if (htim->Instance == TIM2) {
        //tmrTask_thread();

        if (CO->CANmodule[0]->CANnormal) {
            bool_t syncWas;

            /* Process Sync and read inputs */
            syncWas = CO_process_SYNC_RPDO(CO, TMR_TASK_INTERVAL);

            /* Further I/O or nonblocking application code may go here. */
            program1ms();
            /* Write outputs */
            CO_process_TPDO(CO, syncWas, TMR_TASK_INTERVAL);

            /* verify timer overflow */
            if (0) {
                CO_errorReport(CO->em, CO_EM_ISR_TIMER_OVERFLOW, CO_EMC_SOFTWARE_INTERNAL, 0U);
            }
        }
        //xprintf("irq\n");
    }
}


void Error_Handler(void)
{
    /* USER CODE BEGIN Error_Handler_Debug */
    /* User can add his own implementation to report the HAL error return state */

    /* USER CODE END Error_Handler_Debug */
}

#ifdef  USE_FULL_ASSERT
/**
  * @brief  Reports the name of the source file and the source line number
  *         where the assert_param error has occurred.
  * @param  file: pointer to the source file name
  * @param  line: assert_param error line source number
  * @retval None
  */
void assert_failed(uint8_t* file, uint32_t line)
{
    /* USER CODE BEGIN 6 */
    /* User can add his own implementation to report the file name and line number,
       tex: printf("Wrong parameters value: file %s on line %d\r\n", file, line) */
       /* USER CODE END 6 */
}
#endif /* USE_FULL_ASSERT */

/************************ (C) COPYRIGHT STMicroelectronics *****END OF FILE****/
