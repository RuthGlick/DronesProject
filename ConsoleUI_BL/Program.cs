using System;
using static BL.Enum;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;
using System.Linq;

namespace ConsoleUI_BL
{
    class Program
    {
        static void Main(string[] args)
        {
           
            BlApi.IBL bl;
            bl = BlApi.BlFactory.GetBl(); 
            Menu menu;
            do
            {
                Console.WriteLine("enter your choice:");
                foreach (Menu item in Enum.GetValues(typeof(Menu)))
                {
                    Console.WriteLine($"{(int)item} - {item}");
                }
                menu = (Menu)getValidInt();
                SwitchMenu(menu, bl);
            } while (menu != Menu.EXIT);
        }

        static void SwitchMenu(Menu menu, BlApi.IBL bl)
        {
            switch (menu)
            {
                case Menu.ADD:
                    {
                        foreach (Add item in Enum.GetValues(typeof(Add)))
                        {
                            Console.WriteLine($"{(int)item} - {item}");
                        }
                        Add add = (Add)getValidInt();
                        SwitchAdd(add, bl);
                        break;
                    }

                case Menu.UPDATE:
                    {
                        foreach (Update item in Enum.GetValues(typeof(Update)))
                        {
                            Console.WriteLine($"{(int)item} - {item}");
                        }                      
                        Update update = (Update)getValidInt();
                        SwitchUpdate(update, bl);
                        break;
                    }

                case Menu.DISPLAY:
                    {
                        foreach (Display item in Enum.GetValues(typeof(Display)))
                        {
                            Console.WriteLine($"{(int)item} - {item}");
                        }
                        Display display = (Display)getValidInt();
                        SwitchDisplay(display, bl);
                        break;
                    }

                case Menu.VIEW_LIST:
                    {
                        foreach (ViewList item in Enum.GetValues(typeof(ViewList)))
                        {
                            Console.WriteLine($"{(int)item} - {item}");
                        }
                        ViewList viewList = (ViewList)getValidInt();
                        SwitchViewList(viewList, bl);
                        break;
                    }

                case Menu.EXIT:
                    break;
                default:
                    Console.WriteLine("Wrong choice"); 
                    break;
            }
        }

        static void SwitchAdd(Add add, BlApi.IBL bl)
        {
            switch (add)
            {
                case Add.BASE_STATION:
                    {
                        BO.BaseStation b = new BO.BaseStation();
                        Console.WriteLine("enter id, name, location, number of chargeSlots");
                        b.Id = getValidInt();
                        b.Name = Console.ReadLine();
                        b.Location.Longitude = getValidDouble();
                        b.Location.Latitude = getValidDouble();
                        b.AvailableChargeSlots = getValidInt();
                        b.DronesInCharching = new List<BO.DroneInCharging>();
                        try
                        {
                            bl.CreateBaseStation(b);
                            Console.WriteLine("The station was successfully added");
                        }
                        catch(BO.IncorrectInputException e)
                        {
                            Console.WriteLine(e);
                        }
                        catch (BO.ExtantException e)
                        {
                            Console.WriteLine(e);
                        }
                        break;
                    }

                case Add.DRONE:
                    {
                        BO.Drone d = new BO.Drone();
                        Console.WriteLine("enter id and model");
                        d.Id = getValidInt();
                        d.Model = Console.ReadLine();
                        Console.WriteLine("maxWeight:");
                        d.Weight = (WeightCategories)getValidInt();
                        int numStation = getValidInt();
                        try
                        {
                            bl.CreateDrone(d, numStation);
                            Console.WriteLine("the drone was seccessfully added");
                        }
                        catch(BO.UnextantException e)
                        {
                            Console.WriteLine(e);
                        }
                        catch(BO.ExtantException e)
                        {
                            Console.WriteLine(e);
                        }
                        break;
                    }

                case Add.CUSTOMER:
                    {
                        BO.Customer c = new BO.Customer();
                        Console.WriteLine("enter id, name, phoneNumber and location");
                        c.Id = getValidInt();
                        c.Name = Console.ReadLine();
                        c.PhoneNumber = Console.ReadLine();
                        c.Location.Longitude = getValidDouble();
                        c.Location.Latitude = getValidDouble();
                        try
                        {
                            bl.CreateCustomer(c);
                            Console.WriteLine("the customer was successfully added");
                        }
                        catch(BO.ExtantException e)
                        {
                            Console.WriteLine(e);
                        }
                        break;
                    }

                case Add.PARCEL:
                    {
                        BO.Parcel p = new BO.Parcel();
                        Console.WriteLine("enter senderId and targetId");
                        p.Sender.Id = getValidInt();
                        p.Target.Id = getValidInt();
                        Console.WriteLine("maxWeight:");
                        foreach (WeightCategories item in Enum.GetValues(typeof(WeightCategories)))
                        {
                            Console.WriteLine($"{(int)item} - {item}");
                        }
                        p.Weight = (WeightCategories)getValidInt();
                        Console.WriteLine("priority:");
                        foreach (Priorities item in Enum.GetValues(typeof(Priorities)))
                        {
                            Console.WriteLine($"{(int)item} - {item}");
                        }
                        p.Priority = (Priorities)getValidInt();
                        p.Drone1 = null;
                        try
                        {
                            bl.CreateParcel(p);
                            Console.WriteLine("the parcel was successfully added");
                        }
                        catch(BO.UnextantException e)
                        {
                            Console.WriteLine(e);
                        }
                        catch (BO.ExtantException e)
                        {
                            Console.WriteLine(e);
                        }
                        break;
                    }
                default:
                    Console.WriteLine("Wrong choice");
                    break;
            }
        }

        static void SwitchUpdate(Update update, BlApi.IBL bl)
        {
            switch (update)
            {
                case Update.DRONE:
                    {
                        BO.Drone d = new BO.Drone();
                        Console.WriteLine("enter id and new model");
                        d.Id = getValidInt();
                        d.Model = Console.ReadLine();
                        try
                        {
                            bl.UpdateDrone(d);
                            Console.WriteLine("The drone has been successfully updated");
                        }
                        catch(BO.UnextantException e)
                        {
                            Console.WriteLine(e);
                        }
                        break;
                    }

                case Update.BASE_STATION:
                    {
                        BO.BaseStation b = new BO.BaseStation();
                        Console.WriteLine("enter id, name and/or number of chargeSlots");
                        int chargeSlots;
                        b.Id = getValidInt();
                        string str = Console.ReadLine();
                        if (str != "")
                        {
                            b.Name = str;
                        }
                        else
                        {
                            b.Name = "";
                        }
                        bool secceed = int.TryParse(Console.ReadLine(), out int input);
                        if (secceed)
                        {
                            chargeSlots = input;
                        }
                        else
                        {
                            chargeSlots = -1;
                        }
                        try
                        {
                            bl.UpdateBaseStation(b, chargeSlots);
                            Console.WriteLine("The station has been successfully updated");
                        }
                        catch(BO.UnextantException e)
                        {
                            Console.WriteLine(e);
                        }
                        break;
                    }

                case Update.CUSTOMER:
                    {
                        BO.Customer c = new BO.Customer();
                        Console.WriteLine("enter id, new name and/or phoneNumber");
                        c.Id = getValidInt();
                        string str = Console.ReadLine();
                        c.Name = str;
                        str = Console.ReadLine();
                        c.PhoneNumber = str;
                        try
                        {
                            bl.UpdateCustomer(c);
                            Console.WriteLine("The customer has been successfully updated");
                        }
                        catch(BO.UnextantException e)
                        {
                            Console.WriteLine(e);
                        }
                        break;
                    }

                case Update.SENDFORCHARGING:
                    {
                        BO.Drone d = new BO.Drone();
                        Console.WriteLine("enter id");
                        d.Id = getValidInt();
                        try
                        {
                            bl.UpdateCharge(d);
                            Console.WriteLine("The drone was sent for loading");
                        }
                        catch(BO.UnextantException e)
                        {
                            Console.WriteLine(e);
                        }
                        catch(BO.DiscrepanciesException e)
                        {
                            Console.WriteLine(e);
                        }
                        break;
                    }

                case Update.RELEASE:
                    {
                        BO.DroneInCharging d = new BO.DroneInCharging();
                        Console.WriteLine("enter id and time in charging");
                        d.Id = getValidInt();
                        try
                        {
                            bl.UpdateRelease(d, getValidInt());
                            Console.WriteLine("The drone was released from loading");
                        }
                        catch(BO.UnextantException e)
                        {
                            Console.WriteLine(e);
                        }
                        catch(BO.DiscrepanciesException e)
                        {
                            Console.WriteLine(e);
                        }
                        break;
                    }

                case Update.SCHEDULED:
                    {
                        BO.Drone d = new BO.Drone();
                        Console.WriteLine("enter id");
                        d.Id = getValidInt();
                        try
                        {
                            bl.UpdateScheduled(d);
                            Console.WriteLine("The package was associated with a drone");
                        }
                        catch(BO.UnextantException e)
                        {
                            Console.WriteLine(e);
                        }
                        catch(BO.DiscrepanciesException e)
                        {
                            Console.WriteLine(e);
                        }
                        break;
                    }

                case Update.PICKED_UP:
                    {
                        BO.Drone d = new BO.Drone();
                        Console.WriteLine("enter id");
                        d.Id = getValidInt();
                        try
                        {
                            bl.UpdatePickedUp(d);
                            Console.WriteLine("The package was collected by a drone");
                        }
                        catch(BO.UnextantException e)
                        {
                            Console.WriteLine(e);
                        }
                        catch(BO.DiscrepanciesException e)
                        {
                            Console.WriteLine(e);
                        }
                        break;
                    }

                case Update.SUPPLY:
                    {
                        BO.Drone d = new BO.Drone();
                        Console.WriteLine("enter id");
                        d.Id = getValidInt();
                        try
                        {
                            bl.UpdateSupply(d);
                            Console.WriteLine("The package was provided by the drone");
                        }
                        catch(BO.DiscrepanciesException e)
                        {
                            Console.WriteLine(e);
                        }
                        catch(BO.UnextantException e)
                        {
                            Console.WriteLine(e);
                        }
                        break;
                    }
                default:
                    Console.WriteLine("Wrong choice");
                    break;
            }
        }

        static void SwitchDisplay(Display display, BlApi.IBL bl)
        {
            switch (display)
            {
                case Display.BASE_STATION:
                    {
                        BO.BaseStation b = new BO.BaseStation();
                        Console.WriteLine("enter baseStation id");
                        b.Id = getValidInt();
                        try
                        {
                            Console.WriteLine(bl.RequestBaseStation(b));
                        }
                        catch(BO.UnextantException e)
                        {
                            Console.WriteLine(e);
                        }
                        break;
                    }

                case Display.DRONE:
                    {
                        BO.Drone d = new BO.Drone();
                        Console.WriteLine("enter drone id");
                        d.Id = getValidInt();
                        try
                        {
                            Console.WriteLine(bl.RequestDrone(d));
                        }
                        catch(BO.UnextantException e)
                        {
                            Console.WriteLine(e);
                        }
                        catch(BO.DiscrepanciesException e)
                        {
                            Console.WriteLine(e);
                        }
                        break;
                    }

                case Display.CUSTOMER:
                    {
                        BO.Customer c = new BO.Customer();
                        Console.WriteLine("enter customer id");
                        c.Id = getValidInt();
                        try
                        {
                            Console.WriteLine(bl.RequestCustomer(c));
                        }
                        catch(BO.DiscrepanciesException e)
                        {
                            Console.WriteLine(e);
                        }
                        catch(BO.UnextantException e)
                        {
                            Console.WriteLine(e);
                        }
                        break;
                    }

                case Display.PARCEL:
                    {
                        BO.Parcel p = new BO.Parcel();
                        Console.WriteLine("enter parcel id");
                        p.Id = getValidInt();
                        try
                        {
                            Console.WriteLine(bl.RequestParcel(p));
                        }
                        catch(BO.UnextantException e)
                        {
                            Console.WriteLine(e);
                        }
                        break;
                    };
                default:
                    Console.WriteLine("Wrong choice");
                    break;
            }
        }

        static void SwitchViewList(ViewList viewList, BlApi .IBL bl)
        {
            switch (viewList)
            {
                case ViewList.BASE_STATIONS:
                    {
                        IEnumerable<BO.BaseStationForList> baseStations = bl.RequestListBaseStations();
                        foreach (BO.BaseStationForList item in baseStations)
                        {
                            Console.WriteLine(item);
                        }
                        break;
                    }

                case ViewList.DRONES:
                    {
                        IEnumerable<BO.DroneForList> temp = bl.RequestListDrones();
                        foreach (BO.DroneForList item in temp)
                        {
                            Console.WriteLine(item);
                        }
                        break;
                    }

                case ViewList.CUSTOMERS:
                    {
                        IEnumerable<BO.CustomerForList> temp = bl.RequestListCustomers();
                        foreach (BO.CustomerForList item in temp)
                        {
                            Console.WriteLine(item);
                        }
                        break;
                    }

                case ViewList.PARCELS:
                    {
                        try
                        {
                            IEnumerable<BO.ParcelForList> temp = bl.RequestListParcels();
                            foreach (BO.ParcelForList item in temp)
                            {
                                Console.WriteLine(item);
                            }
                        }
                        catch(BO.UnextantException e)
                        {
                            Console.WriteLine(e);
                        }
                        catch(BO.DiscrepanciesException e)
                        {
                            Console.WriteLine(e);
                        }
                        break;
                    }

                case ViewList.PENDING_PARCELS:
                    {
                        try
                        {
                            IEnumerable<BO.ParcelForList> temp = bl.RequestListPendingParcels();
                            foreach (BO.ParcelForList item in temp)
                            {
                                Console.WriteLine(item);
                            }
                        }
                        catch(BO.UnextantException e)
                        {
                            Console.WriteLine(e);
                        }
                        catch(BO.DiscrepanciesException e)
                        {
                            Console.WriteLine(e);
                        }
                        break;
                    }

                case ViewList.AVAILABLE_CHARGESLOTS:
                    {
                        IEnumerable<BO.BaseStationForList> temp = bl.RequestListAvailableChargeSlots();
                        foreach (BO.BaseStationForList item in temp)
                        {
                            Console.WriteLine(item);
                        }
                        break;
                    };
                default:
                    Console.WriteLine("Wrong choice");
                    break;
            }
        }

        private static int getValidInt()
        {
            bool parse = int.TryParse(Console.ReadLine(), out int input);
            while (!parse)
            {
                Console.WriteLine("not valid!!! enter another number");
                parse = int.TryParse(Console.ReadLine(), out input);
            }
            return input;
        }

        private static double getValidDouble()
        {
            bool parse = double.TryParse(Console.ReadLine(), out double input);
            while (!parse)
            {
                Console.WriteLine("not valid!!! enter another number");
                parse = double.TryParse(Console.ReadLine(), out input);
            }
            return input;
        }
    }
}

