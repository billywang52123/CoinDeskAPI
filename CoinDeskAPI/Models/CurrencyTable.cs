using System;
using System.Collections.Generic;

namespace CoinDeskAPI.Models;

public partial class CurrencyTable
{
    public int Id { get; set; }

    public string Currency { get; set; } = null!;

    public string Symbol { get; set; } = null!;

    public string Rate { get; set; } = null!;

    public string Description { get; set; } = null!;

    public double Rate_float { get; set; }

    public string? Chinese { get; set; }

    public string? Japanese { get; set; }

    public string? English { get; set; }

    public DateTime CreateDateTime { get; set; }

    public DateTime UpdateTime { get; set; }
}
