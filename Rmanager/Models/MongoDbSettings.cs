namespace Rmanager.Models
{
    /// <summary>
    /// 2019/10/21 Create
    /// The class used to get mongoDb's settings from appsettings.json
    /// configed in startup.cs
    /// </summary>
    public class DatabaseSettings : IDatabaseSettings
    {
        public string UserCollectionName { get; set; }
        public string RoomCollectionName { get; set; }
        public string OrganizationCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string ImageAssetCollectionName { get; set; }
        public string LogCollectionName { get; set; }
        public string DatabaseName { get; set; }
        public string UserInOrgCollectionName { get; set; }
        public string RoomInOrgCollectionName { get; set; }
        public string BookingRoomCollectionName { get; set; }
}


    public interface IDatabaseSettings
    {
        string UserCollectionName { get; set; }
        string RoomCollectionName { get; set; }
        string OrganizationCollectionName { get; set; }
        string ImageAssetCollectionName { get; set; }
        string LogCollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
        string UserInOrgCollectionName { get; set; }
        string RoomInOrgCollectionName { get; set; }
        string BookingRoomCollectionName { get; set; }
    }
}
