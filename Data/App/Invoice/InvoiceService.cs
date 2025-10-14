namespace SSOPortalX.Data.App.Invoice;

public static class InvoiceService
{
    public static List<InvoiceRecordDto> GetInvoiceRecordList() 
    {
        var dummyUser = new UserDto("", "", DateOnly.FromDateTime(System.DateTime.Now), "", "", new List<PermissionDto>());
        return new()
        {
            new InvoiceRecordDto(dummyUser) { Id = 0, Balance = 205, Total = 3171, Date = DateOnly.FromDateTime(DateTime.Now), State = 1 },
            new InvoiceRecordDto(dummyUser) { Id = 1, Balance = 205, Total = 3171, Date = DateOnly.FromDateTime(DateTime.Now), State = 1 },
            new InvoiceRecordDto(dummyUser) { Id = 2, Balance = 205, Total = 3171, Date = DateOnly.FromDateTime(DateTime.Now), State = 2 },
            new InvoiceRecordDto(dummyUser) { Id = 3, Balance = 205, Total = 3171, Date = DateOnly.FromDateTime(DateTime.Now), State = 3 },
            new InvoiceRecordDto(dummyUser) { Id = 4, Balance = 205, Total = 3171, Date = DateOnly.FromDateTime(DateTime.Now), State = 4 },
            new InvoiceRecordDto(dummyUser) { Id = 5, Balance = 205, Total = 3171, Date = DateOnly.FromDateTime(DateTime.Now), State = 5 },
            new InvoiceRecordDto(dummyUser) { Id = 6, Balance = 205, Total = 3171, Date = DateOnly.FromDateTime(DateTime.Now), State = 1 },
            new InvoiceRecordDto(dummyUser) { Id = 7, Balance = 205, Total = 3171, Date = DateOnly.FromDateTime(DateTime.Now), State = 2 },
            new InvoiceRecordDto(dummyUser) { Id = 8, Balance = 205, Total = 3171, Date = DateOnly.FromDateTime(DateTime.Now), State = 3 },
            new InvoiceRecordDto(dummyUser) { Id = 9, Balance = 205, Total = 3171, Date = DateOnly.FromDateTime(DateTime.Now), State = 4 },
            new InvoiceRecordDto(dummyUser) { Id = 10, Balance = 205, Total = 3171, Date = DateOnly.FromDateTime(DateTime.Now), State = 5 },
            new InvoiceRecordDto(dummyUser) { Id = 11, Balance = 205, Total = 3171, Date = DateOnly.FromDateTime(DateTime.Now), State = 1 },
            new InvoiceRecordDto(dummyUser) { Id = 12, Balance = 205, Total = 3171, Date = DateOnly.FromDateTime(DateTime.Now), State = 2 },
            new InvoiceRecordDto(dummyUser) { Id = 13, Balance = 205, Total = 3171, Date = DateOnly.FromDateTime(DateTime.Now), State = 3 },
            new InvoiceRecordDto(dummyUser) { Id = 14, Balance = 205, Total = 3171, Date = DateOnly.FromDateTime(DateTime.Now), State = 4 },
            new InvoiceRecordDto(dummyUser) { Id = 15, Balance = 205, Total = 3171, Date = DateOnly.FromDateTime(DateTime.Now), State = 5 },
            new InvoiceRecordDto(dummyUser) { Id = 16, Balance = 205, Total = 3171, Date = DateOnly.FromDateTime(DateTime.Now), State = 1 },
            new InvoiceRecordDto(dummyUser) { Id = 17, Balance = 205, Total = 3171, Date = DateOnly.FromDateTime(DateTime.Now), State = 2 },
            new InvoiceRecordDto(dummyUser) { Id = 18, Balance = 205, Total = 3171, Date = DateOnly.FromDateTime(DateTime.Now), State = 3 },
            new InvoiceRecordDto(dummyUser) { Id = 19, Balance = 205, Total = 3171, Date = DateOnly.FromDateTime(DateTime.Now), State = 4 },
            new InvoiceRecordDto(dummyUser) { Id = 20, Balance = 205, Total = 3171, Date = DateOnly.FromDateTime(DateTime.Now), State = 5 },
        };
    }

    public static List<BillDto> GetBillList() => new()
    {
        new BillDto("App Design", 24, 1, 24, "Designed UI kit & app pages."),
        new BillDto("App Customization", 26, 1, 26, "Customization & Bug Fixes."),
        new BillDto("ABC Template", 28, 1, 28, "Bootstrap 4 admin template."),
        new BillDto("App Development", 32, 1, 32, "Native App Development."),
    };

    public static List<InvoiceStateDto> GetStateList() => new()
    {
        new InvoiceStateDto("Downloaded", 1),
        new InvoiceStateDto("Draft", 2),
        new InvoiceStateDto("Paid", 3),
        new InvoiceStateDto("Partial Payment", 4),
        new InvoiceStateDto("Past Due", 5)
    };

    public static PagingData<InvoiceRecordDto> GetInvoiceRecordList(int pageIndex, int pageSize, int state)
    {
        var invoiceRecordList = GetInvoiceRecordList();

        var items = invoiceRecordList
            .Where(a => a.State == state || state == 0)
            .OrderBy(a => a.Id)
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        return new PagingData<InvoiceRecordDto>(pageIndex, pageSize, invoiceRecordList.Count, items);
    }

    public static List<string> GetpaymentMethodList() => new()
    {
        "Cash",
        "Bank Transfer",
        "Debit",
        "Credit",
        "Paypal"
    };
}

