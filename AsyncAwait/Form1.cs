using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AsyncAwait
{
    public partial class Form1 : Form
    {
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        
        CancellationTokenSource cts = new CancellationTokenSource();

        public Form1()
        {
            InitializeComponent();
        }

        private async void btnMainTask_Click(object sender, EventArgs e)
        {
            //List<Task> tasks = new List<Task>();
            for (int i = 0; i < 10000; i++)
            {
                int threadID = System.Threading.Thread.CurrentThread.ManagedThreadId;

                Logger.Info($"{i} - {threadID} - Primeira mensagem");

                // Se usar await, as mensagens ficam todas sequenciadas
                // Se não usar as mensagens saem fora de ordem
                // Para gravar o arquivo teve que tratar exceção por causa de arquivo já aberto
                //tasks.Add(DoSomething(i));
                await DoSomething(i);
                //tasks.Add(DoSomething(i));

                Logger.Info($"{i} - {threadID} - Segunda mensagem");

                lblCount.Text = i.ToString();

                if (cts.Token.IsCancellationRequested)
                {
                    Logger.Error($"{i} - {threadID} - Loop cancelado manualmente");
                    break;
                }
            }
            //await Task.WhenAll(tasks);
            MessageBox.Show("Fim!");
        }

        private Task DoSomething(int i)
        {
            return Task.Run(() => SendMessage(i));
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Logger.Error("Teste msg erro");
        }

        private void SendMessage(int i)
        {
            Logger.Warn($"{i} - {System.Threading.Thread.CurrentThread.ManagedThreadId} - Menssagem Task");
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            cts.Cancel();
        }
    }
}
