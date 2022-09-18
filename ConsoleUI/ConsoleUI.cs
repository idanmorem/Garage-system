using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Ex03.GarageLogic;

namespace Ex03.ConsoleUI
{
     public class ConsoleUi
     {
          private readonly GarageLogicC m_GarageLogic;
          private string m_LastActionMessage;
          private const string m_readLine = "1";

          public static void Main()
          {
               ConsoleUi ui = new ConsoleUi();
               ui.GarageMenu();
          }

          public ConsoleUi()
          {
               m_GarageLogic = new GarageLogicC();
               m_LastActionMessage = null;
          }

          public void GarageMenu()
          {
               bool contBrowsingMenu = true;
               while (contBrowsingMenu)
               {
                    try
                    {
                         Console.Clear();
                         if (m_LastActionMessage != null)
                         {
                              Console.WriteLine("Last action recap: " + Environment.NewLine + m_LastActionMessage);
                         }

                         printMainMenu();
                         string currentUserInput = Console.ReadLine();
                         contBrowsingMenu = getInputForAction(getCurrentOperation(currentUserInput));
                    }
                    catch (KeyNotFoundException e)
                    {
                         m_LastActionMessage = "Error! The entered license plate does not exist in our garage!";
                    }
                    catch (ValueOutOfRangeException e)
                    {
                         m_LastActionMessage = "Error! you've got to enter a value between " + e.MinValue + " to " +
                              e.MaxValue;
                    }
                    catch (Exception e)
                    {
                         m_LastActionMessage = e.Message;
                    }

                    m_LastActionMessage += Environment.NewLine;
               }
               Console.WriteLine("You've chosen to quit, goodbye!~");
          }

          //this method assumes the input is a legal input!
          private bool getInputForAction(GarageLogicC.eGarageOperations i_GetCurrentOperation)
          {
               bool contStatus = true;
               if (i_GetCurrentOperation == GarageLogicC.eGarageOperations.InsertNewVehicle)
               {
                    insertNewVehicleUserConsoleInput();
               }
               else if (i_GetCurrentOperation == GarageLogicC.eGarageOperations.ListLicensedVehicles)
               {
                    getListLicensedVehiclesToConsole();
               }
               else if (i_GetCurrentOperation == GarageLogicC.eGarageOperations.ChangeVehicleState)
               {
                    changeVehicleStateConsoleInput();
               }
               else if (i_GetCurrentOperation == GarageLogicC.eGarageOperations.AddTirePressure)
               {
                    addTirePressureInput();
               }
               else if (i_GetCurrentOperation == GarageLogicC.eGarageOperations.FillGasMotor)
               {
                    fillGasMotor();
               }
               else if (i_GetCurrentOperation == GarageLogicC.eGarageOperations.FillElectricMotor)
               {
                    fillElectricMotorInput();
               }
               else if (i_GetCurrentOperation == GarageLogicC.eGarageOperations.ExhibitSpecificCar)
               {
                    exhibitSpecificCarToConsole();
               }
               else //quit
               {
                    contStatus = false;
               }

               return contStatus;
          }
          private void exhibitSpecificCarToConsole()
          {
               string licensePlateNumber;
               Console.WriteLine("\nPlease enter the license number, followed by an ENTER.");
               Vehicle clonedVehicle = m_GarageLogic.GetVehicleCopy(licensePlateNumber = Console.ReadLine());

               StringBuilder sb = new StringBuilder();
               sb.Append("License Number: ");
               sb.Append(licensePlateNumber);
               sb.Append("\nModel: ");
               sb.Append(clonedVehicle.ModelName);
               sb.Append("\nOwners: ");
               sb.Append(clonedVehicle.OwnersName);
               sb.Append("\nStatus: ");
               sb.Append(clonedVehicle.Status);

               int wheelIndex = 0;
               foreach (Wheel wheel in clonedVehicle.Wheels)
               {
                    sb.Append("\n\nWheel number ");
                    sb.Append(wheelIndex + 1);
                    sb.Append(":\nWheels manufacturer: ");
                    sb.Append(clonedVehicle.Wheels[wheelIndex].ManufacturerName);
                    sb.Append("\nWheels Pressure: ");
                    sb.Append(clonedVehicle.Wheels[wheelIndex].CurrentAirPressure);
                    sb.Append(" psi");
                    wheelIndex++;
               }

               if (clonedVehicle.CurrentEngine is FuelEngine)
               {
                    sb.Append("\n\nEngine fuel percentage is: ");
                    sb.Append(clonedVehicle.CurrentEngine.EnergyPercent);
                    sb.Append("%");
                    sb.Append("\nEngine fuel type is: ");
                    sb.Append((clonedVehicle.CurrentEngine as FuelEngine).FuelType.ToString());
               }
               else if (clonedVehicle.CurrentEngine is ElectricEngine)
               {
                    sb.Append("\n\nEngine battery percentage is: ");
                    sb.Append(clonedVehicle.CurrentEngine.EnergyPercent);
                    sb.Append("%");
               }
               foreach (PropertyInfo vehiclesUniqueProperty in m_GarageLogic.GetVehiclesUniqueProperties(licensePlateNumber))
               {
                    sb.Append("\n" + vehiclesUniqueProperty.Name + ": " + m_GarageLogic.GetStringPropertyValue(clonedVehicle, vehiclesUniqueProperty)); //TODO: get the generically
               }
               sb.Append("\n");
               m_LastActionMessage = sb.ToString();
          }

          private void fillElectricMotorInput()
          {
               Console.WriteLine("\nPlease enter the license number, followed by an ENTER.");
               string licenseNumber = Console.ReadLine();
               m_GarageLogic.CheckIfVehicleExists(licenseNumber);
               m_GarageLogic.CheckIfEngineIsElectric(licenseNumber);
               Console.WriteLine("The current amount of battery hours left is {0} out of {1}", m_GarageLogic.GetAmountOfEnergy(licenseNumber), m_GarageLogic.GetMaxAmountOfEnergy(licenseNumber));
               Console.WriteLine("Please choose the amount of hours to fill, followed by an ENTER.");
               string amountToFill = Console.ReadLine();
               m_GarageLogic.Charge(licenseNumber, float.Parse(amountToFill));
               m_LastActionMessage = "The vehicle hours of battery left has been updated\n";
          }
          private void fillGasMotor()
          {
               Console.WriteLine("\nPlease enter the license number, followed by an ENTER.");
               string licenseNumber = Console.ReadLine();
               m_GarageLogic.CheckIfVehicleExists(licenseNumber);
               m_GarageLogic.CheckIfEngineIsFuel(licenseNumber);
               Console.WriteLine("Please choose a fuel type for the vehicle, followed by an ENTER.");
               int i = 0;
               foreach (FuelEngine.eFuelType type in Enum.GetValues(typeof(FuelEngine.eFuelType)))
               {
                    Console.WriteLine("Press {0} to insert a {1}", i, type.ToString());
                    ++i;
               }
               string newFuelType = Console.ReadLine();
               m_GarageLogic.CheckIfFuelTypeIsCorrect((FuelEngine.eFuelType)Enum.Parse(typeof(FuelEngine.eFuelType), newFuelType), licenseNumber);
               Console.WriteLine("The current amount of fuel is {0} out of {1}", m_GarageLogic.GetAmountOfEnergy(licenseNumber), m_GarageLogic.GetMaxAmountOfEnergy(licenseNumber));
               Console.WriteLine("Please choose the amount of fuel to fill, followed by an ENTER.");
               string amountToFill = Console.ReadLine();
               m_GarageLogic.AddFuel(licenseNumber, (FuelEngine.eFuelType)Enum.Parse(typeof(FuelEngine.eFuelType), newFuelType), float.Parse(amountToFill));
               m_LastActionMessage = "The vehicle fuel amount has been updated\n";
          }
          private void addTirePressureInput()
          {
               Console.WriteLine("Please enter the license number, followed by an ENTER.");

               string userChoice = Console.ReadLine();
               m_GarageLogic.FillWheelsAirPressure(userChoice);
               m_LastActionMessage = "The wheel's air pressure is maximum\n";
          }

          private void changeVehicleStateConsoleInput()
          {
               Console.WriteLine("\nPlease enter the license number, followed by an ENTER.");
               string licenseNumber = Console.ReadLine();
               m_GarageLogic.CheckIfVehicleExists(licenseNumber);

               int i = 0;
               foreach (Vehicle.eVehicleStatus status in Enum.GetValues(typeof(Vehicle.eVehicleStatus)))
               {
                    Console.WriteLine("Press {0} to change the vehicle status to {1}", i, status.ToString());
                    ++i;
               }
               string newStatus = Console.ReadLine();
               checkValidStatusInputAndReturnIntVal(newStatus);
               m_GarageLogic.ChangeStatus(licenseNumber, (Vehicle.eVehicleStatus)Enum.Parse(typeof(Vehicle.eVehicleStatus), newStatus));
               m_LastActionMessage = "The vehicle has successfully changed his status\n";
          }

          private void getListLicensedVehiclesToConsole()
          {
               StringBuilder messageBuilder = new StringBuilder();
               int counter = 1;
               Console.WriteLine("\nHere you can view a list of all the vehicles plate number in our garage.");
               Console.WriteLine("if you wish to display the plate numbers only press 1, otherwise press any other key to continue with filter options.");
               messageBuilder.Append("List Format: Plate Number\n\n");
               if (Console.ReadLine() == "1")
               {
                    foreach (string vehicleLicensePlate in m_GarageLogic.GetPlateList())
                    {
                         AppendVehiclePlateNumber(ref counter, messageBuilder, vehicleLicensePlate);
                    }
               }
               else
               {
                    Console.WriteLine("Please choose the car status you wish to display:");
                    int i = 0;
                    foreach (Vehicle.eVehicleStatus status in Enum.GetValues(typeof(Vehicle.eVehicleStatus)))
                    {
                         Console.WriteLine("Press {0} to display status {1}", i, status.ToString());
                         i++;
                    }
                    string strVehicleStatus = Console.ReadLine();
                    int enumEquivalentVehicleStatus = checkValidStatusInputAndReturnIntVal(strVehicleStatus);

                    foreach (string vehicleLicensePlate in m_GarageLogic.GetPlateList())
                    {
                         if (((int)(m_GarageLogic.GetVehicleState(vehicleLicensePlate))) == enumEquivalentVehicleStatus) //
                         {
                              AppendVehiclePlateNumber(ref counter, messageBuilder, vehicleLicensePlate);
                         }
                    }
                    if (counter == 1)
                    {
                         messageBuilder.Append("Nothing to display\n");
                    }
               }
               m_LastActionMessage = messageBuilder.ToString();
          }

          private int checkValidStatusInputAndReturnIntVal(string i_VehicleStatus)
          {
               if (int.TryParse(i_VehicleStatus, out int parcedStatusNumericInput) == false)
               {
                    throw new FormatException(GarageLogicC.k_ParsingToIntError);
               }

               if (Enum.IsDefined(typeof(Vehicle.eVehicleStatus), parcedStatusNumericInput) == false)
               {
                    throw new ValueOutOfRangeException(GarageLogicC.k_MaxNumberOfStatuses, GarageLogicC.k_MinNumberOfStatuses);
               }

               return parcedStatusNumericInput;
          }

          private void AppendVehiclePlateNumber(ref int i_Counter, StringBuilder i_StringBuilder, string i_LicensePlate)
          {
               i_StringBuilder.Append(i_Counter);
               i_StringBuilder.Append(". PlateNumber: ");
               i_StringBuilder.Append(i_LicensePlate);
               i_StringBuilder.Append("\n");
               ++i_Counter;
          }

          private void insertNewVehicleUserConsoleInput()
          {
               Console.WriteLine("\nPlease choose a vehicle type to add to the garage by the matching numbers, followed by an ENTER.");
               int i = 0;
               foreach (Vehicle.eVehicleType type in Enum.GetValues(typeof(Vehicle.eVehicleType)))
               {
                    Console.WriteLine("Press {0} to insert a {1}", i, type.ToString());
                    ++i;
               }

               string vehicleType = Console.ReadLine();
               checkValidVehicleChoiceOrThrowIfNot(vehicleType);
               Vehicle newVehicle = m_GarageLogic.CreateVehicle((Vehicle.eVehicleType)Enum.Parse(typeof(Vehicle.eVehicleType), vehicleType));

               Console.WriteLine("\nPlease enter the model name.");
               string modelName = Console.ReadLine();
               newVehicle.ModelName = modelName;

               Console.WriteLine("\nPlease enter the license number.");
               string licenseNumber = Console.ReadLine();
               checkIsLicenseNumberInput(licenseNumber);
               m_GarageLogic.CheckIfVehicleNotExists(licenseNumber);

               Console.WriteLine("\nPlease enter the owner name, followed by an ENTER.");
               string ownerName = Console.ReadLine();
               newVehicle.OwnersName = ownerName;

               Console.WriteLine("\nPlease enter the owner number, followed by an ENTER.");
               string phoneNumber = Console.ReadLine();
               checkValidPhoneNumberInput(phoneNumber);
               newVehicle.OwnersPhoneNumber = phoneNumber;

               Console.WriteLine("\nPlease choose an engine type for your vehicle, followed by an ENTER.");
               i = 0;
               foreach (Engine.eEngineType type in Enum.GetValues(typeof(Engine.eEngineType)))
               {
                    Console.WriteLine("Press {0} to insert a {1}", i, type.ToString());
                    ++i;
               }
               string engineType = Console.ReadLine();
               checkValidEngineTypeInput(engineType);
               m_GarageLogic.AddEngine(newVehicle, (Engine.eEngineType)Enum.Parse(typeof(Engine.eEngineType), engineType));

               Console.WriteLine("\nPlease enter the percentage of energy left in the Vehicle, followed by an ENTER.");
               string energyPercentage = Console.ReadLine();
               checkValidEnergyPercentageInput(energyPercentage);
               m_GarageLogic.AddPercentage(newVehicle, float.Parse(energyPercentage));

               Console.WriteLine("\nWelcome to the wheels section:\nif you want to enter the same information for all wheels please press 1, otherwise press any other key.");
               string wheelManufacturerName;
               float currentAirPressure;
               if (Console.ReadLine() == m_readLine)
               {
                    checkValidWheelInput(newVehicle, out wheelManufacturerName, out currentAirPressure);
                    m_GarageLogic.AddWheels(newVehicle, wheelManufacturerName, currentAirPressure);
               }
               else
               {
                    int wheelIndex = 0;
                    foreach (Wheel wheel in newVehicle.Wheels)
                    {
                         Console.WriteLine("Here you will enter information for wheel number {0}:", wheelIndex + 1);
                         checkValidWheelInput(newVehicle, out wheelManufacturerName, out currentAirPressure);
                         m_GarageLogic.AddSingleWheel(newVehicle, wheelManufacturerName, currentAirPressure, wheelIndex);
                         wheelIndex++;
                    }
               }

               specialConditions(newVehicle);

               //i try so hard
               //in the end it doesn't ever matter
               m_GarageLogic.AddVehicle(newVehicle, licenseNumber);
               m_LastActionMessage = "The vehicle has been successfully added to the data base\n";
          }

          private void checkValidWheelInput(Vehicle i_Vehicle, out string i_Manufacturer, out float i_CurrentAirPressure)
          {
               Console.WriteLine("\nPlease enter the Wheel manufacturer name, followed by an ENTER.");
               i_Manufacturer = Console.ReadLine();
               Console.WriteLine("\nThe Wheel's maximum air pressure is: {0}", m_GarageLogic.GetMaxAirPressure(i_Vehicle));
               Console.WriteLine("Please enter the Wheels current air pressure, followed by an ENTER.");
               string strWheelAirPressure = Console.ReadLine();

               if (float.TryParse(strWheelAirPressure, out i_CurrentAirPressure) == false)
               {
                    throw new FormatException(GarageLogicC.k_ParsingToIntError);
               }
          }

          private void specialConditions(Vehicle i_NewVehicle)
          {

               foreach (PropertyInfo uniquePropertyInfo in m_GarageLogic.GetVehiclesUniqueProperties(i_NewVehicle))
               {
                    //prints required data
                    string newPropertyValue;

                    if (uniquePropertyInfo.PropertyType.BaseType == typeof(Enum))
                    {
                         newPropertyValue = handleEnumCase(i_NewVehicle, uniquePropertyInfo);
                    }
                    else if (uniquePropertyInfo.PropertyType == typeof(bool))
                    {
                         newPropertyValue = handleBooleanCase(uniquePropertyInfo);
                    }
                    else
                    {
                         newPropertyValue = handleSingleInputCase(uniquePropertyInfo);
                    }
                    m_GarageLogic.SetValueForUniqueProperty(uniquePropertyInfo, i_NewVehicle, newPropertyValue);
               }
          }
          private string handleBooleanCase(PropertyInfo i_UniquePropertyInfo)
          {
               Console.WriteLine("\nChoosing " + i_UniquePropertyInfo.Name + ":");
               Console.WriteLine("Press 1 if true or 2 if false");
               return Console.ReadLine();
          }

          private string handleSingleInputCase(PropertyInfo i_UniquePropertyInfo)
          {
               Console.WriteLine("Enter the desired " + i_UniquePropertyInfo.Name + ":");
               return Console.ReadLine();
          }

          private string handleEnumCase(Vehicle i_NewVehicle, PropertyInfo i_UniquePropertyInfo)
          {
               Type matchingTypeEnum = i_NewVehicle.getUniqueType(i_UniquePropertyInfo.Name);
               Console.WriteLine("\nChoosing " + i_UniquePropertyInfo.Name + ":");
               getEnumConsoleMessage(matchingTypeEnum);
               //user enters enum choice
               return Console.ReadLine();
          }

          private void getEnumConsoleMessage(Type i_MatchingTypeEnum)
          {
               int i = 0;
               foreach (Enum enumType in Enum.GetValues(i_MatchingTypeEnum))
               {
                    Console.WriteLine("Press " + i + " to pick a " + enumType.ToString());
                    ++i;
               }
          }

          private void checkValidVehicleChoiceOrThrowIfNot(string i_Input)
          {
               if (!int.TryParse(i_Input, out int numericParsedInput))
               {
                    throw new FormatException(GarageLogicC.k_ParsingToIntError);
               }

               if (Enum.IsDefined(typeof(Vehicle.eVehicleType), numericParsedInput) == false)
               {
                    throw new ValueOutOfRangeException(2, 0);
               }
          }
          private void checkIsLicenseNumberInput(string i_Input)
          {
               if (int.TryParse(i_Input, out int numbericParsedInput) == false)
               {
                    throw new FormatException(GarageLogicC.k_ParsingToIntError);
               }

               if (numbericParsedInput <= 0)
               {
                    throw new ArgumentException("You've entered a negative number as a plate number, you nuts?");
               }
          }
          private void checkValidPhoneNumberInput(string i_Input)
          {
               if (int.TryParse(i_Input, out int numbericParsedInput) == false)
               {
                    throw new FormatException(GarageLogicC.k_ParsingToIntError);
               }

               if (numbericParsedInput <= 0)
               {
                    throw new ArgumentException("You've entered a negative number as a phone number, you nuts?");
               }
          }

          private void checkValidEngineTypeInput(string i_Input)
          {
               if (int.TryParse(i_Input, out int parcedEngineNumericInput) == false)
               {
                    throw new FormatException(GarageLogicC.k_ParsingToIntError);
               }

               if (Enum.IsDefined(typeof(Engine.eEngineType), parcedEngineNumericInput) == false)
               {
                    throw new ValueOutOfRangeException(1, 0);
               }

          }

          private void checkValidEnergyPercentageInput(string i_Input)
          {
               if (float.TryParse(i_Input, out float parsedEnteredPercentage) == false)
               {
                    throw new FormatException(GarageLogicC.k_ParsingToIntError);
               }

               if (parsedEnteredPercentage > GarageLogicC.k_MaxPercentage || parsedEnteredPercentage < GarageLogicC.k_MinPercentage)
               {
                    throw new ValueOutOfRangeException(GarageLogicC.k_MaxPercentage, GarageLogicC.k_MinPercentage);
               }
          }

          private GarageLogicC.eGarageOperations getCurrentOperation(string i_CurrentUserInput)
          {
               if (int.TryParse(i_CurrentUserInput, out int parsedUserChoice) == false)
               {

                    throw new FormatException(GarageLogicC.k_ParsingToIntError);
               }
               else
               {
                    if ((parsedUserChoice > GarageLogicC.k_NumberOfAvailableMethodsInGarage || parsedUserChoice <= 0) == true)
                    {
                         throw new ValueOutOfRangeException(GarageLogicC.k_NumberOfAvailableMethodsInGarage, 1);
                    }
               }
               return (GarageLogicC.eGarageOperations)parsedUserChoice;
          }

          private void printMainMenu()
          {
               Console.WriteLine("Welcome to our garage!\n");
               const string k_Selectable = "Selectable actions menu: ";
               const string k_InsertNewCar = "Press 1 to insert a new vehicle into the garage";
               const string k_ExhibitLicensedPlates = "Press 2 to get a filtered by state list of the licensed vehicles within our garage.";
               const string k_ChangeState = "Press 3 to change the state of the the vehicle in the garage.";
               const string k_AddTirePressure = "Press 4 to fill a selected vehicle tires.";
               const string k_FillGas = "Press 5 to fill gas to a gasoline-based engine vehicle.";
               const string k_ChargeElectric = "Press 6 to charge an electric-based engine vehicle";
               const string k_ExhibitSingle = "Press 7 to get an extended observation on a vehicle.";
               const string k_Quit = "Press 8 to end our services.";
               const string k_EnteredChoice = "Please enter your choice now:";

               Console.WriteLine("{0}\n{1}\n{2}\n{3}\n{4}\n{5}\n{6}\n{7}\n{8}\n\n{9}", k_Selectable, k_InsertNewCar, k_ExhibitLicensedPlates, k_ChangeState, k_AddTirePressure, k_FillGas, k_ChargeElectric, k_ExhibitSingle, k_Quit, k_EnteredChoice);
          }
     }
}
