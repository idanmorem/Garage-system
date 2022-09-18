using System;
using System.Reflection;

namespace Ex03.GarageLogic
{
     public class Car : Vehicle
     {
          private eCarColor m_Color;
          private eNumberOfDoors m_NumberOfDoors = eNumberOfDoors.FourDoors;
          private const int k_MaxColorVal = 3;
          private const int k_MinColorVal = 0;
          private const int k_MaxDoorsVal = 3;
          private const int k_MinDoorsVal = 0;

          public Car() : base(Wheel.eNumberOfWheels.FourWheels) { }

          public eCarColor Color
          {
               get => m_Color;
               set
               {
                    if (value <= eCarColor.Black && value >= eCarColor.Red)
                    {
                         m_Color = value;
                    }
                    else
                    {
                         //                         throw new ValueOutOfRangeException(k_MaxColorVal, k_MinColorVal);
                    }
               }
          }

          public eNumberOfDoors NumberOfDoors
          {
               get => m_NumberOfDoors;
               set
               {
                    if (value <= eNumberOfDoors.FiveDoors && value >= eNumberOfDoors.TwoDoors)
                    {
                         m_NumberOfDoors = value;
                    }
                    else
                    {
                         throw new ValueOutOfRangeException(k_MaxDoorsVal, k_MinDoorsVal);
                    }
               }
          }

          public enum eCarColor
          {
               Red,
               Silver,
               White,
               Black
          }

          public enum eNumberOfDoors
          {
               TwoDoors,
               ThreeDoors,
               FourDoors,
               FiveDoors
          }

          public override Type getUniqueType(string i_PropertyName)
          {
               Type specificType;
               if (i_PropertyName == "Color")
               {
                    specificType = typeof(eCarColor);
               }
               else if (i_PropertyName == "NumberOfDoors")
               {
                    specificType = typeof(eNumberOfDoors);
               }
               else
               {
                    throw new ArgumentException("BadType"); //change
               }

               return specificType;
          }

          public override object AutonomicParser(PropertyInfo i_PropertyToBeParsed, object i_ValueToBeParsed)
          {

               object parsedValue = null;
               if (i_ValueToBeParsed != null)
               {
                    string strValue = i_ValueToBeParsed as string;
                    //Wheel wheel in i_Vehicle.Wheels
                    if (Equals(i_PropertyToBeParsed, this.GetType().GetProperty("Color")))
                    {
                         //TODO: check valid input
                         parsedValue = Enum.Parse(typeof(eCarColor), strValue);
                    }
                    else //it's the number of doors
                    {
                         parsedValue = Enum.Parse(typeof(eNumberOfDoors), strValue);
                    }
               }
               else // parsedValue == null -> only return the object string that represents the properties value
               {
                    if (Equals(i_PropertyToBeParsed, this.GetType().GetProperty("Color")))
                    {
                         //TODO: check valid input
                         parsedValue = Color.ToString();
                    }
                    else //it's the number of doors
                    {
                         parsedValue = NumberOfDoors.ToString();
                    }
               }
               return parsedValue;
          }


          public override Vehicle DeepClone()
          {
               Car newCarClone = base.DeepClone() as Car;
               return newCarClone;
          }
     }
}
