using System;
using System.Reflection;

namespace Ex03.GarageLogic
{
     public class Motorcycle : Vehicle
     {
          private eLicenseType m_LicenseType;
          private int m_EngineSize;

          public Motorcycle() : base(Wheel.eNumberOfWheels.TwoWheels) { }


          public int EngineSize
          {
               get => m_EngineSize;
               set
               {
                    if (value <= 0)
                    {
                         throw new ValueOutOfRangeException();
                    }
                    else
                    {
                         m_EngineSize = value;
                    }
               }
          }

          public eLicenseType LicenseType
          {
               get => m_LicenseType;
               set => m_LicenseType = value;
          }

          public override Type getUniqueType(string i_PropertyName)
          {
               Type specificType;
               if (i_PropertyName == "EngineSize")
               {
                    specificType = this.EngineSize.GetType();
               }
               else if (i_PropertyName == "LicenseType")
               {
                    specificType = typeof(eLicenseType);
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
               if (i_ValueToBeParsed != null)
               {
                    //Wheel wheel in i_Vehicle.Wheels
                    var strValue = i_ValueToBeParsed as string;
                    if (Equals(i_PropertyToBeParsed, this.GetType().GetProperty("EngineSize")))
                    {
                         //TODO: check valid input
                         parsedValue = int.Parse(strValue);
                    }
                    else //it's the license type
                    {
                         parsedValue = Enum.Parse(typeof(eLicenseType), strValue);
                    }
               }
               else
               {
                    if (Equals(i_PropertyToBeParsed, this.GetType().GetProperty("EngineSize")))
                    {
                         //TODO: check valid input
                         parsedValue = EngineSize.ToString();
                    }
                    else //it's type license type
                    {
                         parsedValue = LicenseType.ToString();
                    }
               }
               return parsedValue;
          }

          public override Vehicle DeepClone()
          {
               Motorcycle newBikeClone = base.DeepClone() as Motorcycle;
               return newBikeClone;
          }
          public enum eLicenseType
          {
               A,
               B1,
               AA,
               BB
          }
     }
}
