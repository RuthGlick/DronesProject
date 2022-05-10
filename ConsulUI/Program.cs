using System;
using System.Collections.Generic;
using DalApi;
using DO;

namespace ConsulUI
{
    class Program
    {
        private static DalApi.IDal dal;

        static void Main(string[] args)
        {

        }
    }
}
//        IDal dal = new IDal.DalObject();
//        Menu menu;
//            do
//            {
//                Console.WriteLine("enter your choice:");
//                foreach (Menu item in Enum.GetValues(typeof(Menu)))
//                {
//                    Console.WriteLine($"{(int)item} - {item}");
//                }
//                int input;
//                DalObject.DalObject.Valid(out input);
//                            menu = (Menu) input;
//                SwitchMenu(menu, dal);
//            } while (menu != Menu.EXIT) ;
//        }

//        static void SwitchMenu(Menu menu, IDAL.IDal dal)
//{
//    int input;
//    switch (menu)
//    {
//        case Menu.ADD:
//            {
//                foreach (Add item in Enum.GetValues(typeof(Add)))
//                {
//                    Console.WriteLine($"{(int)item} - {item}");
//                }
//                DalObject.DalObject.Valid(out input);
//                Add add = (Add)input;
//                SwitchAdd(add, dal);
//                break;
//            }

//        case Menu.UPDATE:
//            {
//                foreach (Update item in Enum.GetValues(typeof(Update)))
//                {
//                    Console.WriteLine($"{(int)item} - {item}");
//                }
//                DalObject.DalObject.Valid(out input);
//                Update update = (Update)input;
//                SwitchUpdate(update, dal);
//                break;
//            }
//        case Menu.DISPLAY:
//            {
//                foreach (Display item in Enum.GetValues(typeof(Display)))
//                {
//                    Console.WriteLine($"{(int)item} - {item}");
//                }
//                DalObject.DalObject.Valid(out input);
//                Display display = (Display)input;
//                SwitchDisplay(display, dal);
//                break;
//            }
//        case Menu.VIEW_LIST:
//            {
//                foreach (ViewList item in Enum.GetValues(typeof(ViewList)))
//                {
//                    Console.WriteLine($"{(int)item} - {item}");
//                }
//                DalObject.DalObject.Valid(out input);
//                ViewList viewList = (ViewList)input;
//                SwitchViewList(viewList, dal);
//                break;
//            }
//        case Menu.EXIT:
//            break;
//        default:
//            throw new FormatException();

//    }
//}

//static void SwitchAdd(Add add, IDAL.IDal dal)
//{
//    switch (add)
//    {
//        case Add.BASE_STATION:
//            {
//                dal.CreateBaseStation();
//                break;
//            }
//        case Add.DRONE:
//            {
//                dal.CreateDrone();
//                break;
//            }
//        case Add.CUSTOMER:
//            {
//                CustomerDetails(dal);
//                break;
//            }
//        case Add.PARCEL:
//            {
//                ParcelDetails(dal);
//                break;
//            }
//        default:
//            throw new FormatException();
//    }
//}

//static void SwitchUpdate(Update update, IDAL.IDal dal)
//{
//    switch (update)
//    {
//        case Update.SCHEDULED:
//            {
//                Console.WriteLine("enter parcel id");
//                string Id;
//                DalObject.DalObject.Valid(out id);
//                dal.UpdateScheduled(id);
//                break;
//            }
//        case Update.PICKED_UP:
//            {
//                Console.WriteLine("enter parcel id");
//                string Id;
//                DalObject.DalObject.Valid(out id);
//                dal.UpdatePickedUp(id);
//                break;
//            }
//        case Update.SUPPLY:
//            {
//                Console.WriteLine("enter parcel id");
//                string Id;
//                DalObject.DalObject.Valid(out id);
//                dal.UpdateSupply(id);
//                break;
//            }
//        case Update.SENDING:
//            {
//                Console.WriteLine("enter drone id");
//                string Id;
//                DalObject.DalObject.Valid(out id);
//                if (!dal.UpdateCharge(id))
//                {
//                    Console.WriteLine("there wasn't an available charge slot");
//                }
//                break;
//            }
//        case Update.RELEASE:
//            {
//                Console.WriteLine("enter drone id");
//                string Id;
//                DalObject.DalObject.Valid(out id);
//                dal.UpdateRelease(id);
//                break;
//            }
//        default:
//            throw new FormatException();
//    }
//}

//static void SwitchDisplay(Display display, IDAL.IDal dal)
//{
//    switch (display)
//    {
//        case Display.BASE_STATION:
//            {
//                Console.WriteLine("enter baseStation id");
//                string Id;
//                DalObject.DalObject.Valid(out id);
//                Console.WriteLine(dal.RequestBaseStation(id));
//                break;
//            }
//        case Display.DRONE:
//            {
//                Console.WriteLine("enter drone id");
//                string Id;
//                DalObject.DalObject.Valid(out id);
//                Console.WriteLine(dal.RequestDrone(id));
//                break;
//            }
//        case Display.CUSTOMER:
//            {
//                Console.WriteLine("enter customer id");
//                string Id;
//                DalObject.DalObject.Valid(out id);
//                Console.WriteLine(dal.RequestCustomer(id));
//                break;
//            }
//        case Display.PARCEL:
//            {
//                Console.WriteLine("enter parcel id");
//                string Id;
//                DalObject.DalObject.Valid(out id);
//                Console.WriteLine(dal.RequestParcel(id));
//                break;
//            };
//        default:
//            throw new FormatException();
//    }
//}

//static void SwitchViewList(ViewList viewList, IDAL.IDal dal)
//{
//    switch (viewList)
//    {
//        case ViewList.BASE_STATIONS:
//            {
//                IEnumerable<BaseStation> temp = dal.RequestListBaseStations();
//                foreach (BaseStation item in temp)
//                {
//                    Console.WriteLine(item);
//                }
//                break;
//            }
//        case ViewList.DRONES:
//            {
//                IEnumerable<Drone> temp = dal.RequestListDrones();
//                foreach (Drone item in temp)
//                {
//                    Console.WriteLine(item);
//                }
//                break;
//            }
//        case ViewList.CUSTOMERS:
//            {
//                IEnumerable<Customer> temp = dal.RequestListCustomers();
//                foreach (Customer item in temp)
//                {
//                    Console.WriteLine(item);
//                }
//                break;
//            }
//        case ViewList.PARCELS:
//            {
//                IEnumerable<Parcel> temp = dal.RequestListParcels();
//                foreach (Parcel item in temp)
//                {
//                    Console.WriteLine(item);
//                }
//                break;
//            }
//        case ViewList.PENDING_PARCELS:
//            {
//                IEnumerable<Parcel> temp = dal.RequestListPendingParcels();
//                foreach (Parcel item in temp)
//                {
//                    Console.WriteLine(item);
//                }
//                break;
//            }
//        case ViewList.AVAILABLE_CHARGESLOTS:
//            {
//                IEnumerable<BaseStation> temp = dal.RequestListAvailableChargeSlots();
//                foreach (BaseStation item in temp)
//                {
//                    Console.WriteLine(item);
//                }
//                break;
//            };
//        default:
//            throw new FormatException();
//    }
//}

//    //public static void CustomerDetails(IDAL.IDal dal)
//    //{
//    //    Console.WriteLine("enter customer name:");
//    //    string name = Console.ReadLine();
//    //    Console.WriteLine("enter customer phone:");
//    //    string phone = Console.ReadLine();
//    //    Console.WriteLine("enter customer longitude(0-90):");
//    //    double longitude = double.Parse(Console.ReadLine());
//    //    Console.WriteLine("enter customer latitude(0-180):");
//    //    double latitude = double.Parse(Console.ReadLine());
//    //    dal.CreateCustomer(name, phone, longitude, latitude);
//    //}

//    //public static void ParcelDetails(IDAL.IDal dal)
//    //{
//    //    Console.WriteLine("Enter The Id Of The Sender: ");
//    //    int senderId;
//    //    DalObject.DalObject.Valid(out senderId);
//    //    Console.WriteLine("Enter The Id Of The Getter: ");
//    //    int targetId;
//    //    DalObject.DalObject.Valid(out targetId);
//    //    Console.WriteLine("Enter The Weight Of The Parcel: ");
//    //    foreach (WeightCategories item in Enum.GetValues(typeof(WeightCategories)))
//    //    {
//    //        Console.WriteLine($"{(int)item} - {item}");
//    //    }
//    //    WeightCategories weight = (WeightCategories)Console.Read();
//    //    Console.WriteLine("Enter The Status Of The Parcel:");
//    //    foreach (Priorities item in Enum.GetValues(typeof(Priorities)))
//    //    {
//    //        Console.WriteLine($"{(int)item} - {item}");
//    //    }
//    //    Priorities priority = (Priorities)Console.Read();
//    //    dal.CreateParcel(senderId, targetId, weight, priority);
//    //}
//    //}
//}
