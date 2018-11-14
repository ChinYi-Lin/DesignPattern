﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StatePattern
{
    public class PaymentGate
    {
        Product _product;

        internal IPaymentSatus CurrnetProceess { get; set; }

        public PaymentGate(Product p)
        {
            _product = p;
            CurrnetProceess = new InitSatus(this);
        }

        internal PayStatus CurrnetStatus { get; set; }

        /// <summary>
        /// 設置狀態
        /// </summary>
        /// <param name="status"></param>
        public string SetStatus(PayStatus status)
        {
            return CurrnetProceess.SetSatus(status);
        }

        /// <summary>
        /// 跑流程
        /// </summary>
        /// <returns></returns>
        public string RunProcess()
        {
            return CurrnetProceess.Running(_product);
        }
    }

    public interface IPaymentSatus
    {
        string Running(Product p);

        string SetSatus(PayStatus s);
    }


    public class InitSatus : IPaymentSatus
    {
        PaymentGate _gate;
        public InitSatus(PaymentGate g)
        {
            _gate = g;
        }

        public string Running(Product p)
        {
            _gate.CurrnetProceess = new ProcessSatus(_gate);
            return "交易建立中...";
        }

        public string SetSatus(PayStatus status)
        {
          
            string result = "";
            if (status == PayStatus.Init)
                result = "請勿重新建立訂單";
            else
                _gate.CurrnetStatus = status;

            return result;
        }
    }
    public class ProcessSatus : IPaymentSatus
    {
        PaymentGate _gate;
        public ProcessSatus(PaymentGate g)
        {
            _gate = g;
        }
        public string Running(Product p)
        {
            string result = "交易中請稍後";

            if (p.Price > 300)
            {
                result = "物件超過300元 交易取消中";
                _gate.CurrnetProceess = new CancelSatus(_gate);
            }
            else
                _gate.CurrnetProceess = new SuccessSatus(_gate);

            return result;
        }

        public string SetSatus(PayStatus s)
        {
            string result = string.Empty;
            if (s == PayStatus.Init)
                result = "請勿重新建立訂單";
            return result;
        }
    }
    public class CancelSatus : IPaymentSatus
    {
        PaymentGate _gate;
        public CancelSatus(PaymentGate g)
        {
            _gate = g;
        }
        public string Running(Product p)
        {
            return "交易取消完成";
        }

        public string SetSatus(PayStatus s)
        {
            string result = string.Empty;
            if (s == PayStatus.Init)
                result = "訂單取消請勿修改";
            return result;
        }
    }


    public class SuccessSatus : IPaymentSatus
    {
        PaymentGate _gate;
        public SuccessSatus(PaymentGate g)
        {
            _gate = g;
        }

        public string Running(Product p)
        {
            return "交易完成";
        }

        public string SetSatus(PayStatus s)
        {
            string result = string.Empty;
            if (s == PayStatus.Init)
                result = "訂單成功請勿修改";
            return result;
        }
    }
}
