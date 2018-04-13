
namespace MVCModel
{
    public class BaseEntity: IBaseEntity
    {
        public int UserID { get; set; }
        public int UserPositionID { get; set; }

        public System.DateTime CreatedDate { get; set; }
        public System.DateTime EditedDate { get; set; }
    }
}
