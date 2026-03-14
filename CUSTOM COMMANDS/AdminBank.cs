/*
 * UO Wildlands Custom Script
 * Derived from ServUO Core
 * Compiled & Modified by: [Feng / UO Wildlands Team]
 * * Licensed under the GNU General Public License v3.0 (GPL-3.0)
 */
using System;
using Server;
using Server.Commands;
using Server.Accounting;

namespace Server.Commands
{
    public class AdminBank
    {
        public static void Initialize()
        {
            CommandSystem.Register("AdminBank", AccessLevel.Administrator, new CommandEventHandler(AdminBank_OnCommand));
        }

        [Usage("AdminBank <amount>")]
        public static void AdminBank_OnCommand(CommandEventArgs e)
        {
            if (e.Length < 1)
            {
                e.Mobile.SendMessage("Usage: [AdminBank <amount> (use negative to subtract)");
                return;
            }

            // Changed from GetInt64 to GetInt32 to fix the compiler error
            int amount = e.GetInt32(0); 

            e.Mobile.BeginTarget(-1, false, Targeting.TargetFlags.None, (from, targeted) =>
            {
                if (targeted is Mobile)
                {
                    Mobile m = (Mobile)targeted;
                    Account acct = m.Account as Account;

                    if (acct != null)
                    {
                        if (amount >= 0)
                        {
                            // Positive: Add money using the Deposit method
                            acct.DepositGold(amount);
                            from.SendMessage("Deposited {0} gold into {1}'s account.", amount.ToString("N0"), m.Name);
                        }
                        else
                        {
                            // Negative: Subtract money using the Withdraw method
                            int toRemove = Math.Abs(amount);

                            // Check if they actually have enough to withdraw
                            int goldStub;
                            double currentBalance;
                            acct.GetGoldBalance(out goldStub, out currentBalance);

                            if (currentBalance >= toRemove)
                            {
                                acct.WithdrawGold(toRemove);
                                from.SendMessage("Subtracted {0} gold from {1}'s account.", toRemove.ToString("N0"), m.Name);
                            }
                            else
                            {
                                from.SendMessage("They don't have that much gold. Current balance: {0}", currentBalance.ToString("N0"));
                            }
                        }
                        
                        m.SendMessage("Your account bank balance has been updated.");
                    }
                }
                else
                {
                    from.SendMessage("Target a player.");
                }
            });
        }
    }
}
