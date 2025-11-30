namespace WebApp.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IMediator _mediator;
    private readonly IMapper _mapper;
    public HomeController(ILogger<HomeController> logger, IMediator mediator, IMapper mapper)
    {
        _logger = logger;
        _mediator = mediator;
        _mapper = mapper;
    }

    //[Authorize]
    public async Task<ActionResult> Index()
    {
        var suppliers = _mediator.Send(new GetSuppliersQuery()).Result;
        ViewBag.Suppliers = suppliers;

        GetProductsQuery query = new GetProductsQuery();
        var result = await _mediator.Send(query);

        return View(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateProductCommand command)
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
    public async Task<IActionResult> Edit([FromBody] UpdateProductCommand command)
    {
        try
        {
            var result = await _mediator.Send(command);

            return Json(new
            {
                isSuccess = true,
                message = "عملیات ویرایش با موفقیت انجام شد."
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
    public async Task<IActionResult> Delete([FromBody] DeleteProductCommand command)
    {
        try
        {
            var result = await _mediator.Send(command);

            return Json(new
            {
                isSuccess = true,
                message = "عملیات حذف با موفقیت انجام شد."
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

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
