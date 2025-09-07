﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    public IActionResult Dashboard()
    {
        ViewBag.Message = "Welcome, Admin! You have access to the Admin Dashboard.";
        return View();
    }
}