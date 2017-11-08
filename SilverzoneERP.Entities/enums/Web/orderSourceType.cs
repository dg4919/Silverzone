namespace SilverzoneERP.Entities
{
    // this must be matched from inventorysource table
    public enum orderSourceType
    {
        Press = 1,
        Dealer,
        Online,         // for books order
        Branch,
        InHouse,
        School,         // for school orders
        Silverzone,
        Scrap,
        Other,          // for other items (pen, pencil tec) send to school
        Lot
    }

    public enum itemType
    {
       Broucher = 1,
       Other 
    }

    public enum labelType
    {
        None,
        Principal,
        HM,
        Cordinator,
        Other_Name
    }
}
