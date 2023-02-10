using Dapper;
using ManejoPresupuesto.Models;
using ManejoPresupuesto.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;

namespace ManejoPresupuesto.Controllers
{
    public class AccountTypeController : Controller
    {
        private readonly IRepositoryAccountType _repositoryAccount;

        
        public AccountTypeController(IRepositoryAccountType repositoryAccount)
        {
            _repositoryAccount = repositoryAccount;
        }


        public async Task <IActionResult> Index()
        {
            var userId = 1;
            
            var accountType = await _repositoryAccount.GetUserId(userId);
            
            return View(accountType);
        }

        public IActionResult Create()
        {

            return View();
        }



        [HttpPost]
        public async Task<IActionResult> Create(AccountType accountType)
        {
            //validaciones de atributos
            if (ModelState.IsValid)
            {
                return View(accountType);
            }

            //validacion si tipo de cuenta es repetida
            var existAccounType = await _repositoryAccount.Exits(accountType.Name, accountType.UserId);
            
            if (existAccounType)
            {
                ModelState.AddModelError(nameof(accountType.Name),
                    $"El nombre {accountType.Name} ya existe");

                return View(accountType);
            }
            
            accountType.UserId = 1; 
            await _repositoryAccount.Create(accountType);
            
            return RedirectToAction("Index");  //redirecciona a la vista Index
        }
        
        
        [HttpGet]
        public async Task<ActionResult> UpdateAccountTypeId(int id)
        {
            var userId = 1;
            
            var accountType = await _repositoryAccount.GetAccountId(id, userId);
            
            if (accountType is null)
            {
                return RedirectToAction("No encontrado", "Home");
            }
            
            return View(accountType);
        }



        [HttpPost]
        public async Task<ActionResult>Update (AccountType accountType)
        {
            var userId = 1;
            
            var accountTypeExist = await _repositoryAccount.GetAccountId(accountType.AccountTypeId, userId);
            
            if (accountTypeExist is null)
            {
                return RedirectToAction("NotFound", "Home");
            }
            
            await _repositoryAccount.Update(accountType);
            
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var userId = 1;
        
            var accountType = await _repositoryAccount.GetAccountId(id, userId);
            
            if (accountType is null)
            {
                return RedirectToAction("NotFound", "Home");
            }

            return View(accountType);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAccountType(int id)
        {
            
            var userId = 1;
            
            var accountType = await _repositoryAccount.GetAccountId(id, userId);
            
            if (accountType is null)
            {
                return RedirectToAction("NotFound", "Home");
            }

            await _repositoryAccount.Delete(id);
            
            return RedirectToAction("Index");
        }


    }
}
