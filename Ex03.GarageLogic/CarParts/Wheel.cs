namespace Ex03.GarageLogic
{
     public class Wheel
     {
          private string m_ManufacturerName;
          private float m_CurrentAirPressure;
          private float m_MaxAirPressure;

          // private void AddAir(float i_AmountOfAirToAdd)
          // {
          //      if ((m_CurrentAirPressure + i_AmountOfAirToAdd) <= m_MaxAirPressure)
          //      {
          //           m_CurrentAirPressure = m_CurrentAirPressure + i_AmountOfAirToAdd;
          //      }
          //      else
          //      {
          //           throw new ValueOutOfRangeException();
          //      }
          // }

          public string ManufacturerName
          {
               get => m_ManufacturerName;
               set => m_ManufacturerName = value;
          }

          public float CurrentAirPressure
          {
               get => m_CurrentAirPressure;
               set
               {
                    if (value <= m_MaxAirPressure && value >= 0)
                    {
                         m_CurrentAirPressure = value;
                    }
                    else
                    {
                         throw new ValueOutOfRangeException(MaxAirPressure, k_MinAirPressure);
                    }
               }
          }

          private const int k_MinAirPressure = 0;
          public float MaxAirPressure
          {
               get => m_MaxAirPressure;
               set => m_MaxAirPressure = value;
          }

          public enum eNumberOfWheels
          {
               TwoWheels = 2,
               FourWheels = 4,
               SixteenWheels = 16
          }
     }
}
