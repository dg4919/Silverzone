namespace SilverzoneERP.Entities
{
    public enum EventType
    {
        CurrentEvent=1,
        This_Year_All_Event=2,        
        Last_Year_Current_Event=3,
        Last_Year_All_Event =4,
        Participated_Till_Date = 5,
        Not_Participated_Till_Date = 6        
    }
    public enum LotType
    {
        Question_Paper = 1,
        Book = 2
    }
    public enum Level
    {
        Level_1 = 1,
        Level_2 = 2
    }
    public enum LotFilter
    {
        Do_not_consider_Lot = 1,
        In_Any_Lot = 2,
        Not_Any_Lot__Selected_Date = 3,        
        From_Any_lot_other_than_selected_date_force_insert = 4,
        Without_0_Student = 5,
        Without_Student_Difference = 6
    }
    public enum DNDType
    {
        MobileNo=1,
        EmailId=2
    }
    public enum BankType
    {
        Axis = 1,
        Kotak = 2
    }

}
