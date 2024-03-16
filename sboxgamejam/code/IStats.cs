using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sandbox
{
	public interface IStats
	{
		public int Level { get; set; }
		public int Experience { get; set; }
		public float HP { get; set; }
		public float MaxHP { get; set; }
		public int Mana { get; set; }
		public int MaxMana { get; set; }
		public float ManaRegen { get; set; }
		public int PhysicalPower { get; set; }
		public int MindPower { get; set; }
		public int Protection { get; set; }
		public int Fortitude { get; set; }
		public float MovementSpeed { get; set; }
		public float HitSpeed { get; set; }

		public void TakeDamage(Guid attackerGUID);
		public void ActivateDeathState(Guid attackerGUID);
	}
}
