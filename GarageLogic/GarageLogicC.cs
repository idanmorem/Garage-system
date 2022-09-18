using System;
using System.Collections.Generic;
using System.Reflection;

namespace Ex03.GarageLogic
{
     public class GarageLogicC
     {
          private readonly Dictionary<string, Vehicle> r_VehiclesInGarage;
          private readonly VehicleFactory r_Factory;
          public const int k_NumberOfAvailableMethodsInGarage = 8;
          public const string k_ParsingToIntError = "Error! You've been asked to enter a whole number(here's a clue - you haven't).";
          public const float k_MaxPercentage = 100;
          public const float k_MinPercentage = 0;
          public const int k_MaxNumberOfStatuses = 2;
          public const int k_MinNumberOfStatuses = 0;
          public GarageLogicC()
          {
               r_VehiclesInGarage = new Dictionary<string, Vehicle>();
               r_Factory = new VehicleFactory();
          }

          public enum eGarageOperations
          {
               InsertNewVehicle = 1,
               ListLicensedVehicles,
               ChangeVehicleState,
               AddTirePressure,
               FillGasMotor,
               FillElectricMotor,
               ExhibitSpecificCar,
               QuitApp
          }
          public void AddVehicle(Vehicle i_Vehicle, string i_LicenseNumber)
          {
               r_VehiclesInGarage.Add(i_LicenseNumber, i_Vehicle);
          }

          public float GetMaxAirPressure(Vehicle i_Vehicle)
          {
               return i_Vehicle.Wheels[0].MaxAirPressure;
          }

          public void AddEngine(Vehicle i_Vehicle, Engine.eEngineType i_Type)
          {
               r_Factory.AddEngine(i_Vehicle, i_Type);
          }
          public void AddPercentage(Vehicle i_Vehicle, float i_Input)
          {
               i_Vehicle.CurrentEngine.EnergyPercent = i_Input;
               i_Vehicle.CurrentEngine.CalcCurrentEnergy();
          }

          public Vehicle CreateVehicle(Vehicle.eVehicleType i_Type)
          {
               return r_Factory.CreateVehicle(i_Type);
          }

          public void AddFuel(string i_Id, FuelEngine.eFuelType i_FuelType, float i_AmountToFill)
          {
               r_VehiclesInGarage.TryGetValue(i_Id, out Vehicle currentVehicle);
               ((FuelEngine)currentVehicle.CurrentEngine).AddFuel(i_AmountToFill, i_FuelType);
          }
          public void CheckIfVehicleExists(string i_Input)
          {
               if (r_VehiclesInGarage.ContainsKey(i_Input) == false)
               {
                    throw new KeyNotFoundException();
               }
          }

          public void CheckIfVehicleNotExists(string i_Input)
          {
               int parsedInt;

               if (int.TryParse(i_Input, out parsedInt) != true)
               {
                    throw new FormatException(k_ParsingToIntError);
               }
               if (r_VehiclesInGarage.ContainsKey(i_Input) == true)
               {
                    throw new ArgumentException();
               }
          }

          public void AddWheels(Vehicle i_Vehicle, string i_ManufacturerName, float i_CurrentAirPressure)
          {
               foreach (Wheel wheel in i_Vehicle.Wheels)
               {
                    wheel.CurrentAirPressure = i_CurrentAirPressure;
                    wheel.ManufacturerName = i_ManufacturerName;
               }
          }

          public void AddSingleWheel(Vehicle i_Vehicle, string i_ManufacturerName, float i_CurrentAirPressure, int i_Index)
          {
               i_Vehicle.Wheels[i_Index].CurrentAirPressure = i_CurrentAirPressure;
               i_Vehicle.Wheels[i_Index].ManufacturerName = i_ManufacturerName;
          }

          public float GetAmountOfEnergy(string i_LicensePlate)
          {
               r_VehiclesInGarage.TryGetValue(i_LicensePlate, out Vehicle currentVehicle);
               return currentVehicle.CurrentEngine.GetAmountOfEnergy();
          }

          public float GetMaxAmountOfEnergy(string i_LicensePlate)
          {
               r_VehiclesInGarage.TryGetValue(i_LicensePlate, out Vehicle currentVehicle);
               return currentVehicle.CurrentEngine.GetMaxAmountOfEnergy();
          }

          public void CheckIfEngineIsFuel(string i_Input)
          {
               r_VehiclesInGarage.TryGetValue(i_Input, out Vehicle currentVehicle);
               if (!(currentVehicle.CurrentEngine is FuelEngine))
               {
                    throw new ArgumentException();
               }
          }

          public void CheckIfFuelTypeIsCorrect(FuelEngine.eFuelType i_Input, string i_LicensePlate)
          {
               r_VehiclesInGarage.TryGetValue(i_LicensePlate, out Vehicle currentVehicle);
               if (i_Input != (currentVehicle.CurrentEngine as FuelEngine).FuelType)
               {
                    throw new ArgumentException();
               }
          }

          public void CheckIfEngineIsElectric(string i_Input)
          {
               r_VehiclesInGarage.TryGetValue(i_Input, out Vehicle currentVehicle);
               if (!(currentVehicle.CurrentEngine is ElectricEngine))
               {
                    throw new FormatException();
               }
          }

          public void Charge(string i_LicensePlate, float i_AmountToFill)
          {
               r_VehiclesInGarage.TryGetValue(i_LicensePlate, out Vehicle currentVehicle);
               ((ElectricEngine)currentVehicle.CurrentEngine).Charge(i_AmountToFill);
          }

          //assumes the license plate number is valid already
          public void ChangeStatus(string i_VehicleLicensePlate, Vehicle.eVehicleStatus i_NewStatus)
          {
               Vehicle currentVehicle = r_VehiclesInGarage[i_VehicleLicensePlate];
               currentVehicle.Status = i_NewStatus;
          }

          public Dictionary<string, Vehicle>.KeyCollection GetPlateList()
          {
               return r_VehiclesInGarage.Keys;
          }

          public Vehicle.eVehicleStatus GetVehicleState(string i_VehicleLicensePlate)
          {
               r_VehiclesInGarage.TryGetValue(i_VehicleLicensePlate, out Vehicle currentVehicle);
               return currentVehicle.Status;
          }

          public void FillWheelsAirPressure(string i_LicenseNumber)
          {
               Vehicle currentVehicle;
               r_VehiclesInGarage.TryGetValue(i_LicenseNumber, out currentVehicle);
               foreach (Wheel wheel in r_VehiclesInGarage[i_LicenseNumber].Wheels)
               {
                    wheel.CurrentAirPressure = wheel.MaxAirPressure;
               }
          }
          public void SetValueForUniqueProperty(PropertyInfo i_UniquePropertyInfo, Vehicle i_NewVehicle, string i_NewPropertyValue)
          {

               i_UniquePropertyInfo.SetValue(i_NewVehicle, i_NewVehicle.AutonomicParser(i_UniquePropertyInfo, i_NewPropertyValue), null);
          }

          public List<PropertyInfo> GetVehiclesUniqueProperties(Vehicle i_NewVehicle)
          {
               PropertyInfo[] allProperties = i_NewVehicle.GetType().GetProperties();
               List<PropertyInfo> uniqueProperties = new List<PropertyInfo>();
               foreach (PropertyInfo currentCheckedProperty in allProperties)
               {
                    string checkedPropertyName = currentCheckedProperty.Name;

                    if (typeof(Vehicle).GetProperty(checkedPropertyName) == null) //if vehicle doesn't have this property, it's unique.
                    {
                         uniqueProperties.Add(currentCheckedProperty);
                    }
               }

               return uniqueProperties;
          }

          public List<PropertyInfo> GetVehiclesUniqueProperties(string i_LicenseNumber)
          {
               Vehicle copiedVehicle;
               if (r_VehiclesInGarage.TryGetValue(i_LicenseNumber, out copiedVehicle) == false)
               {
                    throw new KeyNotFoundException();
               }
               return GetVehiclesUniqueProperties(r_VehiclesInGarage[i_LicenseNumber]);
          }

          public Vehicle GetVehicleCopy(string i_ReadLine)
          {
               if (r_VehiclesInGarage.TryGetValue(i_ReadLine, out Vehicle copiedVehicle) == false)
               {
                    throw new KeyNotFoundException();
               }
               return copiedVehicle.DeepClone();
          }

          public string GetStringPropertyValue(Vehicle i_ClonedVehicle, PropertyInfo i_VehiclesUniqueProperty)
          {
               return i_ClonedVehicle.AutonomicParser(i_VehiclesUniqueProperty, null) as string;
          }
     }
}
