using System;
using System.Reflection;

namespace Ex03.GarageLogic
{
     class Truck : Vehicle
     {
          private float m_MaxLoad;
          private bool m_IsContainingDangerousMaterials;

          public Truck() : base(Wheel.eNumberOfWheels.SixteenWheels) { }

          public float MaxLoad
          {
               get => m_MaxLoad;
               set
               {
                    if (value < 0)
                    {
                         throw new ValueOutOfRangeException();
                    }
                    else
                    {
                         m_MaxLoad = value;
                    }
               }
          }

          public bool IsContainingDangerousMaterials
          {
               get => m_IsContainingDangerousMaterials;
               set => m_IsContainingDangerousMaterials = value;
          }

          public override Type getUniqueType(string i_PropertyName)
          {
               Type specificType;
               if (i_PropertyName == "IsContainingDangerousMaterials")
               {
                    specificType = this.IsContainingDangerousMaterials.GetType();
               }
               else if (i_PropertyName == "MaxLoad")
               {
                    specificType = this.MaxLoad.GetType();
               }
               else
               {
                    throw new ArgumentException("BadType");
               }

               return specificType;
          }
          public override object AutonomicParser(PropertyInfo i_PropertyToBeParsed, object i_ValueToBeParsed)
          {
               object parsedValue = null;
               string strValue = null;
               //Wheel wheel in i_Vehicle.Wheels
               if (i_ValueToBeParsed != null)
               {
                    if (Equals(i_PropertyToBeParsed, this.GetType().GetProperty("IsContainingDangerousMaterials")))
                    {
                         strValue = i_ValueToBeParsed as string;
                         //TODO: check valid input
                         if (strValue == "1") //TODO: replace with const
                         {
                              parsedValue = true;
                         }
                         else if (strValue == "2") //TODO: replace with const
                         {
                              parsedValue = false;
                         }
                         else
                         {
                              throw new ArgumentException("You can only enter 1 for true OR 2 for false");
                         }

                    }
                    else //it's MaxLoad
                    {
                         if (float.TryParse((string)i_ValueToBeParsed, out float tmpFloatHolder))
                         {
                              parsedValue = tmpFloatHolder;
                         }
                         else
                         {
                              throw new ArgumentException("You've got to enter a float");
                         }
                    }
               }
               else
               {
                    if (Equals(i_PropertyToBeParsed, this.GetType().GetProperty("IsContainingDangerousMaterials")))
                    {
                         //TODO: check valid input
                         parsedValue = IsContainingDangerousMaterials.ToString();
                    }
                    else //it's type maxLoad
                    {
                         parsedValue = MaxLoad.ToString();
                    }
               }

               return parsedValue;
          }

          public override Vehicle DeepClone()
          {
               Truck newTruckClone = base.DeepClone() as Truck;
               return newTruckClone;
          }
     }
}
