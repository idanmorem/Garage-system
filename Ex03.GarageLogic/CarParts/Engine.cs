namespace Ex03.GarageLogic
{
     public abstract class Engine
     {
          private float m_EnergyPercent;

          public float EnergyPercent
          {
               get => m_EnergyPercent;
               set => m_EnergyPercent = value;
          }

          public abstract float CalcEnergyPercent();

          public abstract void CalcCurrentEnergy();

          public abstract float GetAmountOfEnergy();
          public abstract float GetMaxAmountOfEnergy();

          public enum eEngineType
          {
               Fuel,
               Electric
          }

          public Engine ShallowClone()
          {
               return this.MemberwiseClone() as Engine;
          }
     }
}
