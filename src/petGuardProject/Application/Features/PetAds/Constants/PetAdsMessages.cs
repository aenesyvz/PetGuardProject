using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace Application.Features.PetAds.Constants;

public class PetAdsMessages
{
    public const string SectionName = "PetAds";

    public const string PetAdsDontExists = "PetAdsDontExists";
    public const string PetAdsAlreadyExists = "PetAdsAlreadyExists";
    public const string PetAdsNotActive = "PetAdsNotActive";
    public const string PetAdsDeleted = "PetAdsDeleted";
    public const string PetAdsCompleted = "PetAdsCompleted";
    public const string StartDateMustBeBeforeEndDate = "StartDateMustBeBeforeEndDate";

}
