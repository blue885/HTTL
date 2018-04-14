
namespace MVCModel
{
    public interface IBaseEntity
    {
        int UserID { get; set; }
        int UserPositionID { get; set; }

        System.DateTime CreatedDate { get; set; }
        System.DateTime EditedDate { get; set; }
    }
}
