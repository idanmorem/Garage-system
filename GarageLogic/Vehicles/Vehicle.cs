using System;
using System.Reflection;

namespace Ex03.GarageLogic
{
     public abstract class Vehicle
     {
          private string m_ModelName;
          private readonly Wheel[] m_Wheels;
          private readonly Wheel.eNumberOfWheels r_NumberOfWheels;
          private string m_OwnersName;
          private string m_OwnersPhoneNumber;
          private eVehicleStatus m_Status;
          private Engine m_CurrentEngine;

          internal Vehicle(Wheel.eNumberOfWheels i_NumberOfWheels)
          {
               m_Status = eVehicleStatus.InProgress;
               r_NumberOfWheels = i_NumberOfWheels;
               m_Wheels = new Wheel[(int)i_NumberOfWheels];
               for (int i = 0; i < (int)i_NumberOfWheels; i++)
               {
                    m_Wheels[i] = new Wheel();
               }
          }

          public enum eVehicleStatus
          {
               InProgress,
               Fixed,
               Paid
          }

          public Wheel[] Wheels => m_Wheels;

          public string ModelName
          {
               get => m_ModelName;
               set => m_ModelName = value;
          }

          public Wheel.eNumberOfWheels NumberOfWheels => r_NumberOfWheels;

          public string OwnersName
          {
               get => m_OwnersName;
               set => m_OwnersName = value;
          }

          public string OwnersPhoneNumber
          {
               get => m_OwnersPhoneNumber;
               set => m_OwnersPhoneNumber = value;
          }

          public eVehicleStatus Status
          {
               get => m_Status;
               set => m_Status = value;
          }

          public Engine CurrentEngine
          {
               get => m_CurrentEngine;
               set => m_CurrentEngine = value;
          }

          public enum eVehicleType
          {
               Car,
               Motorcycle,
               Truck
          }

          public abstract Type getUniqueType(string i_PropertyName); //<Enter number option, pick-able object

          public abstract object AutonomicParser(PropertyInfo i_PropertyToBeParsed, object valueToBeParsed);

          public virtual Vehicle DeepClone()
          {
               Vehicle cloneVehicle = (Vehicle)this.MemberwiseClone();
               int i = 0;
               foreach (Wheel wheel in cloneVehicle.Wheels)
               {
                    cloneVehicle.Wheels.CopyTo(this.Wheels, i);
               }

               cloneVehicle.CurrentEngine = CurrentEngine.ShallowClone();
               return cloneVehicle;
          }
     }
}
