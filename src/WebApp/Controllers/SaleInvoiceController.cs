namespace WebApp.Controllers;

public class SaleInvoiceController : Controller
{
    private readonly IMediator _mediator;

    public SaleInvoiceController(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<IActionResult> Index()
    {
        try
        {
            var result = await _mediator.Send(new GetSaleFormDataQuery());
            ViewBag.Products = result.Products;
            ViewBag.Customers = result.Customers;

            var data = await _mediator.Send(new GetSaleInvoiceQuery());
            return View(data);
        }
        catch (ValidationException ex)
        {
            return Json(new
            {
                isSuccess = false,
                message = string.Join("\n", ex.Errors.Select(e => e.ErrorMessage))
            });
        }
        catch (Exception ex)
        {
            return Json(new
            {
                isSuccess = false,
                message = ex.Message
            });
        }
    }

    [HttpGet("SaleInvoice/Details/{id}")]
    public async Task<IActionResult> Details(int id)
    {
        try
        {
            var query = new GetSaleInvoiceByIdQuery()
            {
                Id = id
            };
            var result = await _mediator.Send(query);
            return Json(result);

        }
        catch (ValidationException ex)
        {
            return Json(new
            {
                isSuccess = false,
                message = string.Join("\n", ex.Errors.Select(e => e.ErrorMessage))
            });
        }
        catch (Exception ex)
        {
            return Json(new
            {
                isSuccess = false,
                message = ex.Message
            });
        }
    }


    [HttpGet]
    public async Task<IActionResult> Create()
    {
        try
        {
            var result = await _mediator.Send(new GetSaleFormDataQuery());
            ViewBag.Products = result.Products;
            ViewBag.Customers = result.Customers;

            return View();
        }
        catch (ValidationException ex)
        {
            return Json(new
            {
                isSuccess = false,
                message = string.Join("\n", ex.Errors.Select(e => e.ErrorMessage))
            });
        }
        catch (Exception ex)
        {
            return Json(new
            {
                isSuccess = false,
                message = ex.Message
            });
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateSaleInvoicesCommand command)
    {
        try
        {
            var result = await _mediator.Send(command);

            return Json(new
            {
                isSuccess = true,
                message = "عملیات ثبت با موفقیت انجام شد."
            });
        }
        catch (ValidationException ex)
        {
            return Json(new
            {
                isSuccess = false,
                message = string.Join("\n", ex.Errors.Select(e => e.ErrorMessage))
            });
        }
        catch (Exception ex)
        {
            return Json(new
            {
                isSuccess = false,
                message = ex.Message
            });
        }
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        try
        {
            var query = new GetSaleInvoiceByIdQuery
            {
                Id = id
            };
            var invoice = await _mediator.Send(query);
            if (invoice == null)
                return NotFound();


            var result = await _mediator.Send(new GetSaleFormDataQuery());
            ViewBag.Products = result.Products;
            ViewBag.Customers = result.Customers;

            return View(invoice);
        }
        catch (ValidationException ex)
        {
            return Json(new
            {
                isSuccess = false,
                message = string.Join("\n", ex.Errors.Select(e => e.ErrorMessage))
            });
        }
        catch (Exception ex)
        {
            return Json(new
            {
                isSuccess = false,
                message = ex.Message
            });
        }
    }
    [HttpPost]
    public async Task<JsonResult> Edit([FromBody] UpdateSaleInvoicesCommand command)
    {
        try
        {
            var result = await _mediator.Send(command);

            return Json(new
            {
                isSuccess = true,
                message = "عملیات ثبت با موفقیت انجام شد."
            });
        }
        catch (ValidationException ex)
        {
            return Json(new
            {
                isSuccess = false,
                message = string.Join("\n", ex.Errors.Select(e => e.ErrorMessage))
            });
        }
        catch (Exception ex)
        {
            return Json(new
            {
                isSuccess = false,
                message = ex.Message
            });
        }
    }


    [HttpPost]
    public async Task<IActionResult> Search([FromBody] GetSaleInvoiceQuery query)
    {
        try
        {
            var result = await _mediator.Send(query);

            return Json(result.Select(x => new
            {
                id = x.Id,
                invoiceNumber = x.InvoiceNumber,
                customerName = x.CustomerName,
                dateString = x.Date.ToString("yyyy/MM/dd"),
                totalPrice = x.TotalPrice
            }));
        }
        catch (ValidationException ex)
        {
            return Json(new
            {
                isSuccess = false,
                message = string.Join("\n", ex.Errors.Select(e => e.ErrorMessage))
            });
        }
        catch (Exception ex)
        {
            return Json(new
            {
                isSuccess = false,
                message = ex.Message
            });
        }
    }
}