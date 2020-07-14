namespace extid_invite_web
{
    public class AppSettings
    {
        public string TenantId { get; set; }

        public string MSGraphClientID { get; set; }

        public string MSGraphClientSecret { get; set; }
        
        public string InviteRedirectUrl { get; set; }
        public string ExternalUsersGroupID { get; set; }
    }
}