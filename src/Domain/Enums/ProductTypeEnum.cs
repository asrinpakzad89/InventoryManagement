namespace Domain.Enums;

public enum ProductTypeEnum
{
    [Display(Name ="دارو")]
    Drug =1,

    [Display(Name = "مکمل")]
    Supplement = 2,

    [Display(Name = "بهداشتی")]
    Hygiene = 3,

    [Display(Name = "سایر")]
    Other = 4,

}
