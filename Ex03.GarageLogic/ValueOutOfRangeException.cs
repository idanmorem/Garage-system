namespace Ex03.GarageLogic
{
     public class ValueOutOfRangeException : System.Exception
     {
          private float m_MaxValue;
          private float m_MinValue;


          public ValueOutOfRangeException(float i_MaxValue, float i_MinValue)
          {
               MaxValue = i_MaxValue;
               MinValue = i_MinValue;
          }

          public ValueOutOfRangeException()
          {
          }

          public float MaxValue
          {
               get => m_MaxValue;
               set => m_MaxValue = value;
          }

          public float MinValue
          {
               get => m_MinValue;
               set => m_MinValue = value;
          }

     }
}
