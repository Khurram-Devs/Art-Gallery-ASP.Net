using System;
using System.Collections.Generic;
namespace Art_Gallery.Models.DTOs;

public record TopNSoldArtModel(string ArtName, string ArtistName, string ArtImage, int TotalUnitSold);
public record TopNSoldArtsVm(DateTime StartDate, DateTime EndDate, IEnumerable<TopNSoldArtModel> TopNSoldArts);
