using System;


namespace DO
{

    public enum WeightCategories { LIGHT, INTERMEDIATE, HEAVY }

    public enum Priorities { REGULAR, FAST, EMERGENCY }

    public enum Menu
    {
        ADD, UPDATE, DISPLAY, VIEW_List, EXIT,
        VIEW_LIST
    }

    public enum Add { BASE_STATION, DRONE, CUSTOMER, PARCEL }

    public enum Update { SCHEDULED, PICKED_UP, SUPPLY, SENDING, RELEASE }

    public enum Display { BASE_STATION, DRONE, CUSTOMER, PARCEL }

    public enum ViewList { BASE_STATIONS, DRONES, CUSTOMERS, PARCELS, PENDING_PARCELS, AVAILABLE_CHARGESLOTS }


}

