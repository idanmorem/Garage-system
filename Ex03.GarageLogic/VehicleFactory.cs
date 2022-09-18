namespace Ex03.GarageLogic
{
     public class VehicleFactory
     {
          public Vehicle CreateVehicle(Vehicle.eVehicleType i_Type)
          {
               Vehicle newVehicle;
               if (i_Type == Vehicle.eVehicleType.Car)
               {
                    newVehicle = new Car();
                    foreach (Wheel wheel in newVehicle.Wheels)
                    {
                         wheel.MaxAirPressure = 32f;
                    }
               }
               else if (i_Type == Vehicle.eVehicleType.Motorcycle)
               {
                    newVehicle = new Motorcycle();
                    foreach (Wheel wheel in newVehicle.Wheels)
                    {
                         wheel.MaxAirPressure = 30f;
                    }
               }
               else // (i_Type == Vehicle.eVehicleType.Truck)
               {
                    newVehicle = new Truck();
                    foreach (Wheel wheel in newVehicle.Wheels)
                    {
                         wheel.MaxAirPressure = 26f;
                    }
               }
               return newVehicle;
          }

          public void AddEngine(Vehicle i_Vehicle, Engine.eEngineType i_Type)
          {
               if (i_Type == Engine.eEngineType.Fuel)
               {
                    i_Vehicle.CurrentEngine = new FuelEngine();
                    if (i_Vehicle is Car)
                    {
                         (i_Vehicle.CurrentEngine as FuelEngine).FuelType = FuelEngine.eFuelType.Octane95;
                         (i_Vehicle.CurrentEngine as FuelEngine).MaxFuelAmount = 45f;
                    }
                    else if (i_Vehicle is Motorcycle)
                    {
                         (i_Vehicle.CurrentEngine as FuelEngine).FuelType = FuelEngine.eFuelType.Octane98;
                         (i_Vehicle.CurrentEngine as FuelEngine).MaxFuelAmount = 6f;
                    }
                    else if (i_Vehicle is Truck)
                    {
                         (i_Vehicle.CurrentEngine as FuelEngine).FuelType = FuelEngine.eFuelType.Solar;
                         (i_Vehicle.CurrentEngine as FuelEngine).MaxFuelAmount = 120f;
                    }
               }
               else if (i_Type == Engine.eEngineType.Electric)
               {
                    i_Vehicle.CurrentEngine = new ElectricEngine();
                    if (i_Vehicle is Car)
                    {
                         (i_Vehicle.CurrentEngine as ElectricEngine).MaxBatteryTime = 3.2f;
                    }
                    else if (i_Vehicle is Motorcycle)
                    {
                         (i_Vehicle.CurrentEngine as ElectricEngine).MaxBatteryTime = 1.8f;
                    }
                    else if (i_Vehicle is Truck)
                    {
                         throw new System.FormatException();
                    }
               }
               else
               {
                    throw new System.FormatException();
               }
          }
     }
}
