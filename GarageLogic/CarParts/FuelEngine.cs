namespace Ex03.GarageLogic
{
     public class FuelEngine : Engine
     {
          private eFuelType m_FuelType;
          private float m_CurrentFuelAmount;
          private float m_MaxFuelAmount;

          public void AddFuel(float i_AmountOfFuelToAdd, eFuelType i_FuelType)
          {
               CurrentFuelAmount = m_CurrentFuelAmount + i_AmountOfFuelToAdd;
          }

          public override float CalcEnergyPercent()
          {
               return ((m_CurrentFuelAmount / m_MaxFuelAmount) * 100);
          }

          public override void CalcCurrentEnergy()
          {
               m_CurrentFuelAmount = ((m_MaxFuelAmount * EnergyPercent) / 100);
          }

          public override float GetAmountOfEnergy()
          {
               return CurrentFuelAmount;
          }

          public override float GetMaxAmountOfEnergy()
          {
               return MaxFuelAmount;
          }

          public eFuelType FuelType
          {
               get => m_FuelType;
               set => m_FuelType = value;
          }

          public float CurrentFuelAmount
          {
               get => m_CurrentFuelAmount;
               set
               {
                    if (value < m_CurrentFuelAmount)
                    {
                         throw new ValueOutOfRangeException();
                    }
                    else if (value > m_MaxFuelAmount)
                    {
                         throw new System.ArgumentException();
                    }
                    else
                    {
                         m_CurrentFuelAmount = value;
                         EnergyPercent = CalcEnergyPercent();
                    }
               }
          }
          public float MaxFuelAmount
          {
               get => m_MaxFuelAmount;
               set
               {
                    if (value < 0)
                    {
                         throw new ValueOutOfRangeException();
                    }
                    else if (value < m_CurrentFuelAmount)
                    {
                         throw new System.ArgumentException();
                    }
                    else
                    {
                         m_MaxFuelAmount = value;
                    }
               }
          }

          public enum eFuelType
          {
               Solar,
               Octane95,
               Octane96,
               Octane98
          }
     }
}
