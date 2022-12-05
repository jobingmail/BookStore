
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BulkyBookCoreMVC.Models;

namespace BulkyBookCoreMVC.Models.ViewModels
{
	public class ShoppingCartVM
	{
		public List<ShoppingCart> ListCart { get; set; }

		public OrderHeader OrderHeader { get; set; }

		public double CartTotal {  get; set; }
	}
}
