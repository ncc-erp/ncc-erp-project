using Microsoft.AspNetCore.Hosting;
using NccCore.Helper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using ProjectManagement.Helper;
using OfficeOpenXml.FormulaParsing.Excel.Functions.RefAndLookup;
using ProjectManagement.APIs.TimesheetProjects.Dto;
using ProjectManagement.Services.Timesheet.Dto;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Abp.Extensions;
using NccCore.Extension;
using Amazon.Runtime.Internal.Util;
using System.Drawing;

namespace ProjectManagement.Helper
{
    public class ExportInvoicePDFHelper
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        public ExportInvoicePDFHelper(IWebHostEnvironment webHostEnvironment)
        {
            this._hostingEnvironment = webHostEnvironment;
        }
        public FileExportInvoiceDto ExportInvoiceDataPdf( InvoiceData data)
        {
            StringBuilder header = new StringBuilder();
            StringBuilder bodyPart1 = new StringBuilder();
            StringBuilder builder = new StringBuilder();
            StringBuilder footer = new StringBuilder();

            double sumLineTotal = 0;
            string currencyName = "";
            foreach (var tsUser in data.TimesheetUsers)
            {
                var evenClass = (data.TimesheetUsers.IndexOf(tsUser) % 2 == 0) ? "" : "_even";
                builder.Append($@"
                <tr class='row14'>
                    <td class='column0'>&nbsp;</td>
                    <td class='column1 style17{evenClass} null'>{tsUser.FullName}</td>
                    <td class='column2 style29{evenClass} null'>{tsUser.ProjectName}</td>
                    <td class='column3 style29{evenClass} null'>{tsUser.BillRateDisplay.ToString("N2", CultureInfo.InvariantCulture)}</td>
                    <td class='column4 style29{evenClass} null'>{tsUser.CurrencyName}/{tsUser.ChargeTypeDisplay}</td>
                    <td class='column5 style29{evenClass} null'>{tsUser.WorkingDayDisplay.ToString("N2", CultureInfo.InvariantCulture)}</td>
                    <td class='column6 style29{evenClass} null' style='text-align: right;'>{tsUser.LineTotal.ToString("N2", CultureInfo.InvariantCulture)}</td>
                    <td class='column7'>&nbsp;</td>
                </tr>");

                sumLineTotal += tsUser.LineTotal;
                currencyName = tsUser.CurrencyName;
            }


            double totalNet = sumLineTotal;
            double invoiceTotal = totalNet + data.Info.TransferFee - (data.Info.Discount * totalNet) / 100;
          
            long invoiceNumberHTML = data.Info.InvoiceNumber;
            string invoiceDateHTML = data.Info.InvoiceDateStr();
            string invoiceTotalHTML = invoiceTotal.ToString("N2", CultureInfo.InvariantCulture);
            string paymentDueByHTML = data.Info.PaymentDueByStr();
            string clientNameHTML = data.Info.ClientName;
            string clientAddressHTML = data.Info.ClientAddress;
            string totalNetHTML = totalNet.ToString("N2", CultureInfo.InvariantCulture);
            float discountHTML = data.Info.Discount;
            string lineTotalDiscountHTML = ((data.Info.Discount * totalNet) / 100).ToString("N2", CultureInfo.InvariantCulture);
            string transferFeeHTML = (data.Info.TransferFee).ToString("N2", CultureInfo.InvariantCulture);
            string arrPaymentInfoHTML = data.Info.PaymentInfo.Replace("\n", "</br>");

            header.Append($@"<!DOCTYPE html  '-//W3C//DTD HTML 4.01//EN' 'http://www.w3.org/TR/html4/strict.dtd'>
<html>
  <head>
      <meta http-equiv='Content-Type' content='text/html; charset=utf-8'>
      <meta name='generator' content='PhpSpreadsheet, https://github.com/PHPOffice/PhpSpreadsheet'>
      <meta name='company' content='Microsoft Corporation' />
    <style type='text/css'>
      html {{ font-family: Times New Roman, Times, serif; font-size:11pt; background-color:white }}
      a.comment-indicator:hover + div.comment {{ background:#ffd; position:absolute; display:block; border:1px solid black; padding:0.5em }}
      a.comment-indicator {{ background:red; display:inline-block; border:1px solid black; width:0.5em; height:0.5em }}
      div.comment {{ display:none }}
      table {{ border-collapse:collapse; page-break-after:always }}
      .gridlines td {{ border:1px dotted black }}
      .gridlines th {{ border:1px dotted black }}
      .b {{ text-align:center }}
      .e {{ text-align:center }}
      .f {{ text-align:right }}
      .inlineStr {{ text-align:left }}
      .n {{ text-align:right }}
      .s {{ text-align:left }}
      td.style0 {{ vertical-align:middle; border-bottom:none #000000; border-top:none #000000; border-left:none #000000; border-right:none #000000; color:#473530;   font-size:10pt; background-color:white }}
      th.style0 {{ vertical-align:middle; border-bottom:none #000000; border-top:none #000000; border-left:none #000000; border-right:none #000000; color:#473530;   font-size:10pt; background-color:white }}
      td.style1 {{ vertical-align:middle; border-bottom:3px solid #1c1c1c !important;; border-top:none #000000; border-left:none #000000; border-right:none #000000; color:#000000;  font-size:11pt; background-color:white }}
      th.style1 {{ vertical-align:middle; border-bottom:3px solid #1c1c1c !important;; border-top:none #000000; border-left:none #000000; border-right:none #000000; color:#000000;  font-size:11pt; background-color:white }}
      td.style2 {{ vertical-align:middle; text-align:left; padding-left:0px; border-bottom:none #000000; border-top:none #000000; border-left:none #000000; border-right:none #000000; color:#000000;  font-size:10pt; background-color:white }}
      th.style2 {{ vertical-align:middle; text-align:left; padding-left:0px; border-bottom:none #000000; border-top:none #000000; border-left:none #000000; border-right:none #000000; color:#000000;  font-size:10pt; background-color:white }}
      td.style3 {{ vertical-align:middle; border-bottom:none #000000; border-top:none #000000; border-left:none #000000; border-right:none #000000; color:#000000;  font-size:11pt; background-color:white }}
      th.style3 {{ vertical-align:middle; border-bottom:none #000000; border-top:none #000000; border-left:none #000000; border-right:none #000000; color:#000000;  font-size:11pt; background-color:white }}
      td.style4 {{ vertical-align:middle; border-bottom:none #000000; border-top:none #000000; border-left:none #000000; border-right:none #000000; color:#000000;  font-size:20pt; background-color:white }}
      th.style4 {{ vertical-align:middle; border-bottom:none #000000; border-top:none #000000; border-left:none #000000; border-right:none #000000; color:#000000;  font-size:20pt; background-color:white }}
      td.style5 {{ vertical-align:middle; text-align:left; padding-left:0px; border-bottom:none #000000; border-top:none #000000; border-left:none #000000; border-right:none #000000; color:#E37000;  font-size:20pt; background-color:white }}
      th.style5 {{ vertical-align:middle; text-align:left; padding-left:0px; border-bottom:none #000000; border-top:none #000000; border-left:none #000000; border-right:none #000000; color:#E37000;  font-size:20pt; background-color:white }}
      td.style6 {{ vertical-align:middle; border-bottom:1px solid #473530 !important; border-top:1px solid #DED0AF !important; border-left:none #000000; border-right:none #000000; font-weight:bold; color:#000000;   font-size:10pt; background-color:white }}
      th.style6 {{ vertical-align:middle; border-bottom:1px solid #473530 !important; border-top:1px solid #DED0AF !important; border-left:none #000000; border-right:none #000000; font-weight:bold; color:#000000;   font-size:10pt; background-color:white }}
      td.style7 {{ vertical-align:middle; text-align:left; padding-left:0px; border-bottom:1px solid #473530 !important; border-top:1px solid #DED0AF !important; border-left:none #000000; border-right:none #000000; font-weight:bold; color:#000000;   font-size:10pt; background-color:white }}
      th.style7 {{ vertical-align:middle; text-align:left; padding-left:0px; border-bottom:1px solid #473530 !important; border-top:1px solid #DED0AF !important; border-left:none #000000; border-right:none #000000; font-weight:bold; color:#000000;   font-size:10pt; background-color:white }}
      td.style8 {{ vertical-align:middle; border-bottom:none #000000; border-top:none #000000; border-left:none #000000; border-right:none #000000; color:#473530;   font-size:10pt; background-color:white }}
      th.style8 {{ vertical-align:middle; border-bottom:none #000000; border-top:none #000000; border-left:none #000000; border-right:none #000000; color:#473530;   font-size:10pt; background-color:white }}
      td.style9 {{ vertical-align:middle; border-bottom:none #000000; border-top:none #000000; border-left:none #000000; border-right:none #000000; color:#473530;   font-size:10pt; background-color:white }}
      th.style9 {{ vertical-align:middle; border-bottom:none #000000; border-top:none #000000; border-left:none #000000; border-right:none #000000; color:#473530;   font-size:10pt; background-color:white }}
      td.style10 {{ vertical-align:bottom; text-align:right; padding-right:0px; border-bottom:none #000000; border-top:none #000000; border-left:none #000000; border-right:none #000000; color:#473530;   font-size:10pt; background-color:white }}
      th.style10 {{ vertical-align:bottom; text-align:right; padding-right:0px; border-bottom:none #000000; border-top:none #000000; border-left:none #000000; border-right:none #000000; color:#473530;   font-size:10pt; background-color:white }}
      td.style11 {{ vertical-align:middle; border-bottom:3px solid #1c1c1c !important;; border-top:none #000000; border-left:none #000000; border-right:none #000000; color:#473530;   font-size:10pt; background-color:white }}
      th.style11 {{ vertical-align:middle; border-bottom:3px solid #1c1c1c !important;; border-top:none #000000; border-left:none #000000; border-right:none #000000; color:#473530;   font-size:10pt; background-color:white }}
      td.style12 {{ vertical-align:bottom; text-align:right; padding-right:9px; border-bottom:3px solid #1c1c1c !important;; border-top:none #000000; border-left:none #000000; border-right:none #000000; font-weight:bold; color:#00679A;  font-size:16pt; background-color:white }}
      th.style12 {{ vertical-align:bottom; text-align:right; padding-right:9px; border-bottom:3px solid #1c1c1c !important;; border-top:none #000000; border-left:none #000000; border-right:none #000000; font-weight:bold; color:#00679A;  font-size:16pt; background-color:white }}
      td.style13 {{ vertical-align:middle; border-bottom:3px solid #1c1c1c !important;; border-top:none #000000; border-left:none #000000; border-right:none #000000; font-weight:bold; font-style:italic; color:#473530;   font-size:10pt; background-color:white }}
      th.style13 {{ vertical-align:middle; border-bottom:3px solid #1c1c1c !important;; border-top:none #000000; border-left:none #000000; border-right:none #000000; font-weight:bold; font-style:italic; color:#473530;   font-size:10pt; background-color:white }}
      td.style14 {{ vertical-align:middle; border-bottom:none #000000; border-top:none #000000; border-left:none #000000; border-right:none #000000; color:#969696;  font-size:11pt; background-color:white }}
      th.style14 {{ vertical-align:middle; border-bottom:none #000000; border-top:none #000000; border-left:none #000000; border-right:none #000000; color:#969696;  font-size:11pt; background-color:white }}
      td.style15 {{ vertical-align:middle; text-align:center; border-bottom:none #000000; border-top:none #000000; border-left:none #000000; border-right:none #000000; color:#000000;  font-size:11pt; background-color:white }}
      th.style15 {{ vertical-align:middle; text-align:center; border-bottom:none #000000; border-top:none #000000; border-left:none #000000; border-right:none #000000; color:#000000;  font-size:11pt; background-color:white }}
      td.style16 {{ vertical-align:middle;font-weight: bold; text-align:center; border-bottom:none #000000; border-top:none #000000; border-left:none #000000; border-right:none #000000; color:#000000;  font-size:10pt; background-color:#FAD5A5  }}
      th.style16 {{ vertical-align:middle; text-align:center; border-bottom:none #000000; border-top:none #000000; border-left:none #000000; border-right:none #000000; color:#000000;  font-size:10pt; background-color:white }}
      td.style17 {{ vertical-align:middle; border-bottom:none #000000; border-top:none #000000; border-left:none #000000; border-right:none #000000; color:#000000;   font-size:10pt; background-color:white }}
      td.style17_even {{ vertical-align:middle; border-bottom:none #000000; border-top:none #000000; border-left:none #000000; border-right:none #000000; color:#000000;   font-size:10pt; background-color:#FFF5EE }}
      th.style17 {{ vertical-align:middle; border-bottom:none #000000; border-top:none #000000; border-left:none #000000; border-right:none #000000; color:#000000;   font-size:10pt; background-color:white }}
      td.style18 {{ vertical-align:middle; border-bottom:none #000000; border-top:none #000000; border-left:none #000000; border-right:none #000000; color:#000000;   font-size:10pt; background-color:white }}
      th.style18 {{ vertical-align:middle; border-bottom:none #000000; border-top:none #000000; border-left:none #000000; border-right:none #000000; color:#000000;   font-size:10pt; background-color:white }}
      td.style19 {{ vertical-align:bottom; text-align:right; padding-right:9px; border-bottom:none #000000; border-top:none #000000; border-left:none #000000; border-right:none #000000; color:#473530;   font-size:10pt; background-color:white }}
      th.style19 {{ vertical-align:bottom; text-align:right; padding-right:9px; border-bottom:none #000000; border-top:none #000000; border-left:none #000000; border-right:none #000000; color:#473530;   font-size:10pt; background-color:white }}
      td.style20 {{ vertical-align:middle; text-align:left; padding-left:10px; font-weight: bold; border-bottom:none #000000; border-top:none #000000; border-left:none #000000; border-right:none #000000; color:#473530;   font-size:10pt; background-color:white }}
      th.style20 {{ vertical-align:middle; text-align:right; padding-right:9px; border-bottom:none #000000; border-top:none #000000; border-left:none #000000; border-right:none #000000; color:#473530;   font-size:10pt; background-color:white }}
      td.style21 {{ vertical-align:top; border-bottom:none #000000; border-top:none #000000; border-left:none #000000; border-right:none #000000; color:#000000;   font-size:10pt; background-color:white }}
      th.style21 {{ vertical-align:top; border-bottom:none #000000; border-top:none #000000; border-left:none #000000; border-right:none #000000; color:#000000;   font-size:10pt; background-color:white }}
      td.style22 {{ vertical-align:bottom; text-align:right; padding-right:9px; border-bottom:none #000000; border-top:none #000000; border-left:none #000000; border-right:none #000000; color:#473530;   font-size:10pt; background-color:white }}
      th.style22 {{ vertical-align:bottom; text-align:right; padding-right:9px; border-bottom:none #000000; border-top:none #000000; border-left:none #000000; border-right:none #000000; color:#473530;   font-size:10pt; background-color:white }}
      td.style23 {{ vertical-align:middle; border-bottom:none #000000; border-top:1px solid #7F7F7F !important; border-left:none #000000; border-right:none #000000; color:#000000;  font-size:11pt; background-color:white }}
      th.style23 {{ vertical-align:middle; border-bottom:none #000000; border-top:1px solid #7F7F7F !important; border-left:none #000000; border-right:none #000000; color:#000000;  font-size:11pt; background-color:white }}
      td.style24 {{ vertical-align:bottom; text-align:left; padding-left:0px; border-bottom:none #000000; border-top:none #000000; border-left:none #000000; border-right:none #000000; color:#AA5400;  font-size:12pt; background-color:white }}
      th.style24 {{ vertical-align:bottom; text-align:left; padding-left:0px; border-bottom:none #000000; border-top:none #000000; border-left:none #000000; border-right:none #000000; color:#AA5400;  font-size:10pt; background-color:white }}
      td.style25 {{ vertical-align:middle; border-bottom:none #000000; border-top:none #000000; border-left:none #000000; border-right:none #000000; color:#AA5400;  font-size:10pt; background-color:white }}
      th.style25 {{ vertical-align:middle; border-bottom:none #000000; border-top:none #000000; border-left:none #000000; border-right:none #000000; color:#AA5400;  font-size:10pt; background-color:white }}
      td.style26 {{ vertical-align:bottom; text-align:right; padding-right:0px; border-bottom:none #000000; border-top:none #000000; border-left:none #000000; border-right:none #000000; color:#AA5400;  font-size:10pt; background-color:white }}
      th.style26 {{ vertical-align:bottom; text-align:right; padding-right:0px; border-bottom:none #000000; border-top:none #000000; border-left:none #000000; border-right:none #000000; color:#AA5400;  font-size:10pt; background-color:white }}
      td.style27 {{ vertical-align:middle; border-bottom:none #000000; border-top:none #000000; border-left:none #000000; border-right:none #000000; color:#473530;  font-size:7pt; background-color:white }}
      th.style27 {{ vertical-align:middle; border-bottom:none #000000; border-top:none #000000; border-left:none #000000; border-right:none #000000; color:#473530;  font-size:7pt; background-color:white }}
      td.style28 {{ vertical-align:middle; border-bottom:none #000000; border-top:none #000000; border-left:none #000000; border-right:none #000000; color:#473530;   font-size:10pt; background-color:white }}
      th.style28 {{ vertical-align:middle; border-bottom:none #000000; border-top:none #000000; border-left:none #000000; border-right:none #000000; color:#473530;   font-size:10pt; background-color:white }}
      td.style29 {{ vertical-align:middle; text-align:center; border-bottom:none #000000; border-top:none #000000; border-left:none #000000; border-right:none #000000; color:#000000;   font-size:10pt; background-color:white }}
      td.style29_even {{ vertical-align:middle; text-align:center; border-bottom:none #000000; border-top:none #000000; border-left:none #000000; border-right:none #000000; color:#000000;   font-size:10pt; background-color:#FFF5EE }}
      th.style29 {{ vertical-align:middle; text-align:center; border-bottom:none #000000; border-top:none #000000; border-left:none #000000; border-right:none #000000; color:#000000;   font-size:10pt; background-color:white }}
      td.style30 {{ vertical-align:middle; border-bottom:none #000000; border-top:3px solid #1c1c1c !important;; border-left:none #000000; border-right:none #000000; color:#000000;   font-size:10pt; background-color:white }}
      th.style30 {{ vertical-align:middle; border-bottom:none #000000; border-top:3px solid #1c1c1c !important;; border-left:none #000000; border-right:none #000000; color:#000000;   font-size:10pt; background-color:white }}
      td.style31 {{ vertical-align:middle; border-bottom:none #000000; border-top:none #000000; border-left:none #000000; border-right:none #000000; color:#473530;   font-size:10pt; background-color:white }}
      th.style31 {{ vertical-align:middle; border-bottom:none #000000; border-top:none #000000; border-left:none #000000; border-right:none #000000; color:#473530;   font-size:10pt; background-color:white }}
      td.style32 {{ vertical-align:middle; text-align:left; padding-left:0px; border-bottom:none #000000; border-top:none #000000; border-left:none #000000; border-right:none #000000; color:#000000;  font-size:10pt; background-color:white }}
      th.style32 {{ vertical-align:middle; text-align:left; padding-left:0px; border-bottom:none #000000; border-top:none #000000; border-left:none #000000; border-right:none #000000; color:#000000;  font-size:10pt; background-color:white }}
      td.style33 {{ vertical-align:middle; border-bottom:none #000000; border-top:none #000000; border-left:none #000000; border-right:none #000000; color:#000000;  font-size:11pt; background-color:white }}
      th.style33 {{ vertical-align:middle; border-bottom:none #000000; border-top:none #000000; border-left:none #000000; border-right:none #000000; color:#000000;  font-size:11pt; background-color:white }}
      td.style34 {{ vertical-align:middle; text-align:center; border-bottom:none #000000; border-top:none #000000; border-left:none #000000; border-right:none #000000; color:#000000;  font-size:10pt; background-color:white }}
      th.style34 {{ vertical-align:middle; text-align:center; border-bottom:none #000000; border-top:none #000000; border-left:none #000000; border-right:none #000000; color:#000000;  font-size:10pt; background-color:white }}
      td.style35 {{ vertical-align:middle; border-bottom:none #000000; border-top:none #000000; border-left:none #000000; border-right:none #000000; color:#473530;   font-size:10pt; background-color:white }}
      th.style35 {{ vertical-align:middle; border-bottom:none #000000; border-top:none #000000; border-left:none #000000; border-right:none #000000; color:#473530;   font-size:10pt; background-color:white }}
      td.style36 {{ vertical-align:middle; border-bottom:none #000000; border-top:none #000000; border-left:none #000000; border-right:none #000000; color:#000000;   font-size:10pt; background-color:white }}
      th.style36 {{ vertical-align:middle; border-bottom:none #000000; border-top:none #000000; border-left:none #000000; border-right:none #000000; color:#000000;   font-size:10pt; background-color:white }}
      td.style37 {{ vertical-align:middle; text-align:center; border-bottom:none #000000; border-top:none #000000; border-left:none #000000; border-right:none #000000; color:#000000;   font-size:10pt; background-color:white }}
      th.style37 {{ vertical-align:middle; text-align:center; border-bottom:none #000000; border-top:none #000000; border-left:none #000000; border-right:none #000000; color:#000000;   font-size:10pt; background-color:white }}
      td.style38 {{ vertical-align:middle; text-align:left; padding-left:0px; border-bottom:none #000000; border-top:none #000000; border-left:none #000000; border-right:none #000000; color:#000000;   font-size:10pt; background-color:white }}
      th.style38 {{ vertical-align:middle; text-align:left; padding-left:0px; border-bottom:none #000000; border-top:none #000000; border-left:none #000000; border-right:none #000000; color:#000000;   font-size:10pt; background-color:white }}
      td.style39 {{ vertical-align:middle; text-align:left; padding-left:0px; border-bottom:none #000000; border-top:none #000000; border-left:none #000000; border-right:none #000000; color:#000000;   font-size:10pt; background-color:white }}
      th.style39 {{ vertical-align:middle; text-align:left; padding-left:0px; border-bottom:none #000000; border-top:none #000000; border-left:none #000000; border-right:none #000000; color:#000000;   font-size:10pt; background-color:white }}
      td.style40 {{ vertical-align:middle; border-bottom:none #000000; border-top:none #000000; border-left:none #000000; border-right:none #000000; color:#473530;   font-size:10pt; background-color:white }}
      th.style40 {{ vertical-align:middle; border-bottom:none #000000; border-top:none #000000; border-left:none #000000; border-right:none #000000; color:#473530;   font-size:10pt; background-color:white }}
      td.style41 {{ vertical-align:middle; text-align:center; border-bottom:none #000000; border-top:none #000000; border-left:none #000000; border-right:none #000000; color:#473530;   font-size:10pt; background-color:white }}
      th.style41 {{ vertical-align:middle; text-align:center; border-bottom:none #000000; border-top:none #000000; border-left:none #000000; border-right:none #000000; color:#473530;   font-size:10pt; background-color:white }}
      td.style42 {{ vertical-align:middle; text-align:center; border-bottom:none #000000; border-top:none #000000; border-left:none #000000; border-right:none #000000; color:#473530;   font-size:10pt; background-color:white }}
      th.style42 {{ vertical-align:middle; text-align:center; border-bottom:none #000000; border-top:none #000000; border-left:none #000000; border-right:none #000000; color:#473530;   font-size:10pt; background-color:white }}
      td.style43 {{ vertical-align:middle; border-bottom:none #000000; border-top:none #000000; border-left:none #000000; border-right:none #000000; color:#E37000;  font-size:20pt; background-color:white }}
      th.style43 {{ vertical-align:middle; border-bottom:none #000000; border-top:none #000000; border-left:none #000000; border-right:none #000000; color:#E37000;  font-size:20pt; background-color:white }}
      td.style44 {{ vertical-align:middle; text-align:left; padding-left:10px; font-weight: bold; border-bottom:none #000000; border-top:none #000000; border-left:none #000000; border-right:none #000000; color:#473530;   font-size:10pt; background-color:white }}
      th.style44 {{ vertical-align:middle; text-align:right; padding-right:9px; border-bottom:none #000000; border-top:none #000000; border-left:none #000000; border-right:none #000000; color:#473530;   font-size:10pt; background-color:white }}
      td.style45 {{ vertical-align:middle; text-align:center; font-weight: bold; border-bottom:none #000000; border-top:none #000000; border-left:none #000000; border-right:none #000000; color:#473530;   font-size:10pt; background-color:white }}
      th.style45 {{ vertical-align:middle; text-align:center; border-bottom:none #000000; border-top:none #000000; border-left:none #000000; border-right:none #000000; color:#473530;   font-size:10pt; background-color:white }}
      td.style46 {{ vertical-align:middle; text-align:right; padding-right:0px; border-bottom:none #000000; border-top:none #000000; border-left:none #000000; border-right:none #000000; font-weight:bold; color:#473530;   font-size:12pt; background-color:white }}
      th.style46 {{ vertical-align:middle; text-align:right; padding-right:0px; border-bottom:none #000000; border-top:none #000000; border-left:none #000000; border-right:none #000000; font-weight:bold; color:#473530;   font-size:10pt; background-color:white }}
      td.style47 {{ vertical-align:top; text-align:right; padding-right:0px; border-bottom:none #000000; border-top:none #000000; border-left:none #000000; border-right:none #000000; color:#473530;   font-size:10pt; background-color:white }}
      th.style47 {{ vertical-align:top; text-align:right; padding-right:0px; border-bottom:none #000000; border-top:none #000000; border-left:none #000000; border-right:none #000000; color:#473530;   font-size:10pt; background-color:white }}
      td.style48 {{ vertical-align:middle; text-align:center; border-bottom:none #000000; border-top:1px solid #473530 !important; border-left:none #000000; border-right:none #000000; font-weight:bold; color:#473530;   font-size:10pt; background-color:white }}
      th.style48 {{ vertical-align:middle; text-align:center; border-bottom:none #000000; border-top:1px solid #473530 !important; border-left:none #000000; border-right:none #000000; font-weight:bold; color:#473530;   font-size:10pt; background-color:white }}
      td.style49 {{ vertical-align:middle; text-align:center; border-bottom:none #000000; border-top:3px solid #1c1c1c !important;; border-left:none #000000; border-right:none #000000; color:#E37000;  font-size:12pt; background-color:white }}
      th.style49 {{ vertical-align:middle; text-align:center; border-bottom:none #000000; border-top:3px solid #1c1c1c !important;; border-left:none #000000; border-right:none #000000; color:#E37000;  font-size:12pt; background-color:white }}
      td.style50 {{ vertical-align:middle; text-align:center; border-bottom:1px solid #473530 !important; border-top:none #000000; border-left:none #000000; border-right:none #000000; color:#E37000;  font-size:15pt; background-color:white }}
      th.style50 {{ vertical-align:middle; text-align:center; border-bottom:1px solid #473530 !important; border-top:none #000000; border-left:none #000000; border-right:none #000000; color:#E37000;  font-size:12pt; background-color:white }}
      td.style51 {{ vertical-align:middle; text-align:center; border-bottom:none #000000; border-top:3px solid #1c1c1c !important;; border-left:none #000000; border-right:none #000000; color:#E37000;  font-size:12pt; background-color:white }}
      th.style51 {{ vertical-align:middle; text-align:center; border-bottom:none #000000; border-top:3px solid #1c1c1c !important;; border-left:none #000000; border-right:none #000000; color:#E37000;  font-size:12pt; background-color:white }}
      td.style52 {{ vertical-align:middle; text-align:center; border-bottom:1px solid #473530 !important; border-top:none #000000; border-left:none #000000; border-right:none #000000; color:#E37000;  font-size:15pt; background-color:white }}
      th.style52 {{ vertical-align:middle; text-align:center; border-bottom:1px solid #473530 !important; border-top:none #000000; border-left:none #000000; border-right:none #000000; color:#E37000;  font-size:12pt; background-color:white }}
      td.style53 {{ vertical-align:middle; text-align:right; padding-right:9px; border-bottom:none #000000; border-top:1px solid #7F7F7F !important; border-left:none #000000; border-right:none #000000; color:#E37000;  font-size:11pt; background-color:white }}
      th.style53 {{ vertical-align:middle; text-align:right; padding-right:9px; border-bottom:none #000000; border-top:1px solid #7F7F7F !important; border-left:none #000000; border-right:none #000000; color:#E37000;  font-size:11pt; background-color:white }}
      td.style54 {{ vertical-align:middle; text-align:center; padding-right:9px; border-bottom:3px solid #1c1c1c !important;; border-top:none #000000; border-left:none #000000; border-right:none #000000; color:#E37000;  font-size:13pt; background-color:white }}
      th.style54 {{ vertical-align:middle; text-align:right; padding-right:9px; border-bottom:3px solid #1c1c1c !important;; border-top:none #000000; border-left:none #000000; border-right:none #000000; color:#E37000;  font-size:11pt; background-color:white }}
      td.style55 {{ vertical-align:middle; text-align:left; padding-left:0px; border-bottom:1px solid #DED0AF !important; border-top:3px solid #1c1c1c !important;; border-left:none #000000; border-right:none #000000; color:#000000;   font-size:10pt; background-color:white }}
      th.style55 {{ vertical-align:middle; text-align:left; padding-left:0px; border-bottom:1px solid #DED0AF !important; border-top:3px solid #1c1c1c !important;; border-left:none #000000; border-right:none #000000; color:#000000;   font-size:10pt; background-color:white }}
      td.style56 {{ vertical-align:middle; text-align:left; padding-left:0px; border-bottom:none #000000; border-top:none #000000; border-left:none #000000; border-right:none #000000; color:#473530;   font-size:10pt; background-color:white }}
      th.style56 {{ vertical-align:middle; text-align:left; padding-left:0px; border-bottom:none #000000; border-top:none #000000; border-left:none #000000; border-right:none #000000; color:#473530;   font-size:10pt; background-color:white }}
      td.style57 {{ vertical-align:middle; text-align:left; padding-left:0px; border-bottom:none #000000; border-top:none #000000; border-left:none #000000; border-right:none #000000; color:#473530;   font-size:10pt; background-color:white }}
      th.style57 {{ vertical-align:middle; text-align:left; padding-left:0px; border-bottom:none #000000; border-top:none #000000; border-left:none #000000; border-right:none #000000; color:#473530;   font-size:10pt; background-color:white }}
      td.style58 {{ vertical-align:middle; text-align:left; padding-left:0px; border-bottom:none #000000; border-top:none #000000; border-left:none #000000; border-right:none #000000; font-weight:bold; color:#473530;   font-size:10pt; background-color:white }}
      th.style58 {{ vertical-align:middle; text-align:left; padding-left:0px; border-bottom:none #000000; border-top:none #000000; border-left:none #000000; border-right:none #000000; font-weight:bold; color:#473530;   font-size:10pt; background-color:white }}
      td.style59 {{ vertical-align:middle; text-align:center; border-bottom:none #000000; border-top:none #000000; border-left:none #000000; border-right:none #000000; color:#473530;  font-size:20pt; background-color:white }}
      th.style59 {{ vertical-align:middle; text-align:center; border-bottom:none #000000; border-top:none #000000; border-left:none #000000; border-right:none #000000; color:#473530;  font-size:20pt; background-color:white }}
      td.style60 {{ vertical-align:bottom; text-align:left; padding-left:0px; border-bottom:3px solid #1c1c1c !important;; border-top:none #000000; border-left:none #000000; border-right:none #000000; color:#473530;   font-size:10pt; background-color:white }}
      th.style60 {{ vertical-align:bottom; text-align:left; padding-left:0px; border-bottom:3px solid #1c1c1c !important;; border-top:none #000000; border-left:none #000000; border-right:none #000000; color:#473530;   font-size:10pt; background-color:white }}
      td.style61 {{ vertical-align:middle; text-align:center; border-bottom:none #000000; border-top:1px solid #7F7F7F !important; border-left:none #000000; border-right:none #000000; color:#E37000;  font-size:11pt; background-color:white }}
      th.style61 {{ vertical-align:middle; text-align:center; border-bottom:none #000000; border-top:1px solid #7F7F7F !important; border-left:none #000000; border-right:none #000000; color:#E37000;  font-size:11pt; background-color:white }}
      td.style62 {{ vertical-align:middle; text-align:center; border-bottom:3px solid #1c1c1c !important;; border-top:none #000000; border-left:none #000000; border-right:none #000000; color:#E37000;  font-size:13pt; background-color:white }}
      th.style62 {{ vertical-align:middle; text-align:center; border-bottom:3px solid #1c1c1c !important;; border-top:none #000000; border-left:none #000000; border-right:none #000000; color:#E37000;  font-size:11pt; background-color:white }}
      table.sheet0 col.col0 {{ width:18.97777756pt }}
      table.sheet0 col.col1 {{ width:108.78888787pt }}
      table.sheet0 col.col2 {{ width:112.78888787pt }}
      table.sheet0 col.col3 {{ width:69.81111031pt }}
      table.sheet0 col.col4 {{ width:80.5555547pt }}
      table.sheet0 col.col5 {{ width:93.53333226pt }}
      table.sheet0 col.col6 {{ width:88.78888787pt }}
      table.sheet0 col.col7 {{ width:43.37777728pt }}
      table.sheet0 col.col8 {{ width:94.8888878pt }}
      table.sheet0 col.col9 {{ width:43.37777728pt }}
      table.sheet0 tr {{ height:10.5pt }}
      table.sheet0 tr.row0 {{ height:27pt }}
      table.sheet0 tr.row1 {{ height:43.5pt }}
      table.sheet0 tr.row2 {{ height:24pt }}
      table.sheet0 tr.row3 {{ height:24pt }}
      table.sheet0 tr.row9 {{ height:12.75pt }}
      table.sheet0 tr.row10 {{ height:6.75pt }}
      table.sheet0 tr.row11 {{ height:6pt }}
      table.sheet0 tr.row12 {{ height:15pt }}
      table.sheet0 tr.row13 {{ height:22pt }}
      table.sheet0 tr.row14 {{ height:18.75pt }}
      table.sheet0 tr.row15 {{ height:18pt }}
      table.sheet0 tr.row16 {{ height:18pt }}
      table.sheet0 tr.row17 {{ height:18pt }}
      table.sheet0 tr.row18 {{ height:18pt }}
      table.sheet0 tr.row19 {{ height:18pt }}
      table.sheet0 tr.row20 {{ height:15pt }}
      table.sheet0 tr.row21 {{ height:15pt }}
      table.sheet0 tr.row22 {{ height:15pt }}
      table.sheet0 tr.row23 {{ height:27pt }}
      #paymentDetail{{opacity: 0.7; transition: opacity 0.5s ease;}}
      #paymentDetail:hover {{opacity: 1;}}
    </style>
  </head>");

            bodyPart1.Append($@" <body>
<style>
@media print {{ @page {{ margin-left: 0.25in; margin-right: 0.05in; margin-top: 0; margin-bottom: 0; }} @page :header {{ display: none; }} @page :footer {{ display: none; }} }}
body {{ margin-left: 0.25in; margin-right: 0.25in; margin-top: 0.5in; margin-bottom: 0.5in;}}
 
</style>
    <table border='0' cellpadding='0' cellspacing='0' id='sheet0' class='sheet0'>
        <col class='col0'>
        <col class='col1'>
        <col class='col2'>
        <col class='col3'>
        <col class='col4'>
        <col class='col5'>
        <col class='col6'>
        <col class='col7'>
      
        <tbody>
          <tr class='row0'>
            <td class='column0'>&nbsp;</td>
            <td class='column1'>&nbsp;</td>
            <td class='column2'>&nbsp;</td>
            <td class='column3'>&nbsp;</td>
            <td class='column4'>&nbsp;</td>
            <td class='column5'>&nbsp;</td>
            <td class='column6'>&nbsp;</td>
            <td class='column7'>&nbsp;</td>
           
           
          </tr>
          <tr class='row1'>
            <td class='column0'>&nbsp;</td>
            <td class='column1 style4 s'>INVOICE</td>
            <td class='column2 style43 null'></td>
            <td class='column3 style43 '>{invoiceNumberHTML}</td>
            <td class='column4 style5 null'></td>
            <td class='column5 style4 null'></td>
            <td class='column6 style4 null'></td>
            <td class='column7'>&nbsp;</td>
           
           
          </tr>
          <tr class='row2'>
            <td class='column0'>&nbsp;</td>
            <td class='column1 style55 n style55' colspan='2'>{invoiceDateHTML}</td>
            <td class='column3 style30 null'></td>
            <td class='column4 style30 null'></td>
            <td class='column5 style51 null style52' rowspan='2'>{currencyName}</td>
            <td class='column6 style49 null style50' rowspan='2'>{invoiceTotalHTML}</td>
            <td class='column7'>&nbsp;</td>
           
           
          </tr>
          <tr class='row3'>
            <td class='column0'>&nbsp;</td>
            <td class='column1 style6 s'>PAYMENT DUE BY: </td>
            <td class='column2 style6 s' >{paymentDueByHTML}</td>
            <td class='column3 style6 null'></td>
            <td class='column4 style7 null'></td>
            <td class='column7'>&nbsp;</td>
           
           
          </tr>
          <tr class='row4'>
            <td class='column0'>&nbsp;</td>
            <td class='column1'>&nbsp;</td>
            <td class='column2'>&nbsp;</td>
            <td class='column3'>&nbsp;</td>
            <td class='column4'>&nbsp;</td>
            <td class='column5'>&nbsp;</td>
            <td class='column6'>&nbsp;</td>
            <td class='column7'>&nbsp;</td>
           
           
          </tr>
          <tr class='row5'>
            <td class='column0'>&nbsp;</td>
            <td class='column1 style58 s style58' colspan='3'>NCCPLUS VIET NAM JOINT STOCK COMPANY</td>
            <td class='column4 style46 s style46' colspan='3'>{clientNameHTML}</td>
            <td class='column7'>&nbsp;</td>
           
           
          </tr>
          <tr class='row6'>
            <td class='column0'>&nbsp;</td>
            <td class='column1 style56 s style56' colspan='3'>No 3B Lane 69, Nguyen Phuc Lai Street</td>
            <td class='column4 style47 s style47' colspan='3' rowspan='3'>{clientAddressHTML}</td>
            <td class='column7'>&nbsp;</td>
           
           
          </tr>
          <tr class='row7'>
            <td class='column0'>&nbsp;</td>
            <td class='column1 style57 s style57' colspan='3'>O Cho Dua Precinct, Dong Da District</td>
            <td class='column7'>&nbsp;</td>
           
           
          </tr>
          <tr class='row8'>
            <td class='column0'>&nbsp;</td>
            <td class='column1 style57 s style57' colspan='3'>Ha Noi, Viet Nam</td>
            <td class='column7'>&nbsp;</td>
           
           
          </tr>
          <tr class='row9'>
            <td class='column0'>&nbsp;</td>
            <td class='column1 style9 null'></td>
            <td class='column2 style9 null'></td>
            <td class='column3 style9 null'></td>
            <td class='column4 style8 null'></td>
            <td class='column5'>&nbsp;</td>
            <td class='column6 style10 null'></td>
            <td class='column7'>&nbsp;</td>
           
           
          </tr>
          <tr class='row10'>
            <td class='column0'>&nbsp;</td>
            <td class='column1 style9 null'></td>
            <td class='column2 style9 null'></td>
            <td class='column3 style9 null'></td>
            <td class='column4 style8 null'></td>
            <td class='column5'>&nbsp;</td>
            <td class='column6 style10 null'></td>
            <td class='column7'>&nbsp;</td>
           
           
          </tr>
          <tr class='row11'>
            <td class='column0'>&nbsp;</td>
            <td class='column1 style11 null'></td>
            <td class='column2 style11 null'></td>
            <td class='column3 style11 null'></td>
            <td class='column4 style1 null'></td>
            <td class='column5 style12 null'></td>
            <td class='column6 style13 null'></td>
            <td class='column7'>&nbsp;</td>
           
           
          </tr>
          <tr class='row12'>
            <td class='column0'>&nbsp;</td>
            <td class='column1 style14 null'></td>
            <td class='column2 style14 null'></td>
            <td class='column3 style14 null'></td>
            <td class='column4 style15 null'></td>
            <td class='column5'>&nbsp;</td>
            <td class='column6'>&nbsp;</td>
            <td class='column7'>&nbsp;</td>
           
           
          </tr>
          <tr class='row13'>
            <td class='column0'>&nbsp;</td>
            <td class='column1 style16 s' style='text-align: center;' >NAME</td>
            <td class='column2 style16 s' >PROJECT</td>
            <td class='column3 style16 s' >RATE</td>
            <td class='column4 style16 s' >RATE TYPE</td>
            <td class='column5 style16 s' >WORKING TIME</td>
            <td class='column6 style16 s' style='text-align: right;'>LINE TOTAL</td>
            <td class='column7'>&nbsp;</td>
           
           
          </tr> ");

            footer.Append(
            #region HTML_Template
           
            $@"
          <tr class='row15'>
            <td class='column0'>&nbsp;</td>
            <td class='column1 style18 null'></td>
            <td class='column2 style18 null'></td>
            <td class='column3 style18 null'></td>
            <td class='column4 style19 null'></td>
            <td class='column5 style20 s'>Net Total</td>
            <td class='column6 style45 f' style='text-align: right;'>{totalNetHTML}</td>
            <td class='column7'>&nbsp;</td>
           
           
          </tr>
          <tr class='row16'>
            <td class='column0'>&nbsp;</td>
            <td class='column1 style21 null'></td>
            <td class='column2 style21 null'></td>
            <td class='column3 style21 null'></td>
            <td class='column4 style22 null'></td>
            <td class='column5 style20 s'>Discount ({discountHTML}%)</td>
            <td class='column6 style45 f' style='text-align: right;'>{lineTotalDiscountHTML}</td>
            <td class='column7'>&nbsp;</td>
           
           
          </tr>
          <tr class='row17'>
            <td class='column0'>&nbsp;</td>
            <td class='column1 style21 null'></td>
            <td class='column2 style21 null'></td>
            <td class='column3 style21 null'></td>
            <td class='column4 style22 null'></td>
            <td class='column5 style44 s'>Transfer Fee</td>
            <td class='column6 style45 n' style='text-align: right;'>{transferFeeHTML}</td>
            <td class='column7'>&nbsp;</td>
           
           
          </tr>
          <tr class='row18'>
            <td class='column0'>&nbsp;</td>
            <td class='column1 style23 null'></td>
            <td class='column2 style23 null'></td>
            <td class='column3 style23 null'></td>
            <td class='column4 style23 null'></td>
            <td class='column5 style53 null style54' rowspan='2'>{currencyName} TOTAL</td>
            <td class='column6 style61 null style62' rowspan='2' style='text-align: right;'>{invoiceTotalHTML}</td>
            <td class='column7'>&nbsp;</td>
           
           
          </tr>
          <tr class='row19'>
            <td class='column0'>&nbsp;</td>
            <td class='column1 style1 null'></td>
            <td class='column2 style1 null'></td>
            <td class='column3 style1 null'></td>
            <td class='column4 style1 null'></td>
            <td class='column7'>&nbsp;</td>
           
           
          </tr>
          <tr class='row20'>
            <td class='column0'>&nbsp;</td>
            <td class='column1'>&nbsp;</td>
            <td class='column2'>&nbsp;</td>
            <td class='column3'>&nbsp;</td>
            <td class='column4'>&nbsp;</td>
            <td class='column5'>&nbsp;</td>
            <td class='column6'>&nbsp;</td>
            <td class='column7'>&nbsp;</td>
           
           
          </tr>
          <tr class='row21'>
            <td class='column0'>&nbsp;</td>
            <td class='column1 style24 s' colspan='2'>PAYMENT DETAILS</td>
            <td class='column2 style24 null'></td>
            <td class='column3 style24 null'></td>
            <td class='column4 style25 null'></td>
            <td class='column5 style25 null'></td>
            <td class='column6 style26 null'></td>
            <td class='column7'>&nbsp;</td>
           
           
          </tr>
          <tr class='row22'>
            <td class='column0'>&nbsp;</td>
              <td class='column1 style57' id='paymentDetail' colspan='8'>
                     <p>
                        {arrPaymentInfoHTML}
                     </p>
                    <p>
                      Payment Reference: {invoiceNumberHTML}
                    </p>
                    <br>
              </td>
          </tr>
          <tr class='row23'>
            <td class='column0'>&nbsp;</td>
            <td class='column1 style48 null style48' colspan='6'></td>
            <td class='column7 style27 null'></td>
           
          </tr>
        </tbody>
    </table>
  </body>
</html>
");
            #endregion

            string htmlDecode =
                header.ToString().Replace("\r\n", "").Replace("\n", "") + 
                bodyPart1.ToString().Replace("\r\n", "").Replace("\n", "") + 
                builder.ToString().Replace("\r\n", "").Replace("\n", "") + 
                footer.ToString().Replace("\r\n", "").Replace("\n", "");
            return new FileExportInvoiceDto {
                FileName = data.ExportFileNameAsPDF(),
                Html = htmlDecode,
                Message = "Success" };
        }
    }
}
