using Application.Features.PurchaseInvoices.Commands.Update;

namespace WebApp.Controllers;

public class PurchaseInvoiceController : Controller
{
    private readonly IMediator _mediator;

    public PurchaseInvoiceController(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<IActionResult> Index()
    {
        var result = await _mediator.Send(new GetPurchaseFormDataQuery());
        ViewBag.Products = result.Products;
        ViewBag.Suppliers = result.Suppliers;

        var data = await _mediator.Send(new GetPurchaseInvoiceQuery());
        return View(data);
    }

    [HttpGet("PurchaseInvoice/Details/{id}")]
    public async Task<IActionResult> Details(int id)
    {
        var query = new GetPurchaseInvoiceByIdQuery()
        {
            Id = id
        };
        var result = await _mediator.Send(query);
        return Json(result);
    }


    [HttpGet]
    public async Task<IActionResult> Create()
    {
        try
        {
            var data = await _mediator.Send(new GetPurchaseFormDataQuery());
            ViewBag.Products = data.Products;
            ViewBag.Suppliers = data.Suppliers;

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
    public async Task<IActionResult> Create([FromBody] CreatePurchaseInvoiceCommand command)
    {
        try
        {
            var result = await _mediator.Send(command);

            return Json(new
            {
                isSuccess = true,
                message = "فاکتور با موفقیت ثبت شد."
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
            var query = new GetPurchaseInvoiceByIdQuery
            {
                Id = id
            };

            var invoice = await _mediator.Send(query);
            if (invoice == null)
                return NotFound();


            var data = await _mediator.Send(new GetPurchaseFormDataQuery());
            ViewBag.Products = data.Products;
            ViewBag.Suppliers = data.Suppliers;

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
    public async Task<JsonResult> Edit([FromBody] UpdatePurchaseInvoiceCommand command)
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
    public async Task<IActionResult> Search([FromBody] GetPurchaseInvoiceQuery query)
    {
        try
        {
            var result = await _mediator.Send(query);

            return Json(result.Select(x => new
            {
                id = x.Id,
                invoiceNumber = x.InvoiceNumber,
                supplierName = x.SupplierName,
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