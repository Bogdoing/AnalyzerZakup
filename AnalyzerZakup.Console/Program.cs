void DateGetMount()
{
    // currDate DD:MM:GGGG hh:mm:ss
    var currDate = DateTime.Now.ToShortDateString() + ", " + DateTime.Now.ToLongTimeString();
    
    // currDate DD:MM:GGGG
    currDate = DateTime.Now.ToShortDateString();

    Console.WriteLine("Current Date - " + currDate);
    //                 Current Date - 18.01.2023, 17:31:49

    Console.WriteLine("Current Date Mount - " + currDate.Split('.')[1]);
    //                 Current Date - 01
}
DateGetMount();

