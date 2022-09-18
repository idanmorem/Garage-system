using System;

namespace Ex03.GarageLogic
{
     public class ElectricEngine : Engine
     {
          private float m_BatteryTimeLeft;
          private float m_MaxBatteryTime;
          public void Charge(float i_AmountOfTimeToAdd)
          {
               BatteryTimeLeft = m_BatteryTimeLeft + i_AmountOfTimeToAdd;
          }

          public override float CalcEnergyPercent()
          {
               return ((m_BatteryTimeLeft / m_MaxBatteryTime) * 100);
          }

          public override void CalcCurrentEnergy()
          {
               m_BatteryTimeLeft = ((m_MaxBatteryTime * EnergyPercent) / 100);
          }

          public override float GetAmountOfEnergy()
          {
               return BatteryTimeLeft;
          }

          public override float GetMaxAmountOfEnergy()
          {
               return MaxBatteryTime;
          }

          public float BatteryTimeLeft
          {
               get => m_BatteryTimeLeft;
               set
               {
                    if (value < m_BatteryTimeLeft)
                    {
                         throw new ArgumentException();
                    }
                    else if (value > m_MaxBatteryTime)
                    {
                         throw new ArgumentException();
                    }
                    else
                    {
                         m_BatteryTimeLeft = value;
                         EnergyPercent = CalcEnergyPercent();
                    }
               }
          }
          public float MaxBatteryTime
          {
               get => m_MaxBatteryTime;
               set
               {
                    if (value < 0)
                    {
                         throw new ArgumentException();
                    }
                    else if (value < m_BatteryTimeLeft)
                    {
                         throw new ArgumentException();
                    }
                    else
                    {
                         m_MaxBatteryTime = value;
                    }
               }
          }
     }
}
