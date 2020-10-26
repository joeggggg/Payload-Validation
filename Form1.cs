using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PayloadValidation
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public static bool calStep1()
        {
            truck truckN = new truck();
            chassis chassisN = new chassis();
            job jobN = new job();
            bool isvalid = false;
            if (jobN.containersWeight > (truckN.truckTareWeight + chassisN.chassisTareWeight + chassisN.chassisAuthorizedPayload) * 1.1)
            {
                isvalid = false;
            }
            else
            {
                isvalid = true;
            }

            return isvalid;
        }

        public static bool calStep2()
        {
            truck truckN = new truck();
            chassis chassisN = new chassis();
            job jobN = new job();
            bool isvalid = false;
            if (jobN.containersWeight > (truckN.truckTareWeight + truckN.truckAuthorizedPayload + jobN.driverWeight) * 1.1)
            {
                isvalid = false;
            }
            else
            {
                isvalid = true;
            }
            return isvalid;
        }

        public static bool calStep3()
        {
            truck truckN = new truck();
            chassis chassisN = new chassis();
            job jobN = new job();
            bool isvalid = false;
            if ((truckN.truckTareWeight + chassisN.chassisTareWeight + jobN.containersWeight) > jobN.authorizedPayload * 1.1)
            {
                isvalid = false;
            }
            else
            {
                isvalid = true;
            }
            return isvalid;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //need to add validation
            truck truckN = new truck();
            truckN.truckNo = tbTruckNo.ToString();
            truckN.truckTareWeight = int.Parse(tbTruckTareWeight.Text);
            truckN.truckAuthorizedPayload = int.Parse(tbTruckAuthorizedPayload.Text);
            truckN.truckAxle = int.Parse(tbTruckAxleQty.Text);

            chassis chassisN = new chassis();
            chassisN.chassisNo = tbChassisNo.ToString();
            chassisN.chassisTareWeight = int.Parse(tbChassisTareWeight.Text);
            chassisN.chassisAuthorizedPayload = int.Parse(tbChassisAuthorizedPayload.Text);
            chassisN.chassisAxle = int.Parse(tbChassisAxleQty.Text);

            job jobN = new job();
            jobN.containersWeight = double.Parse(tbContainerWeightSum.Text);
            jobN.driverWeight = int.Parse(tbDriverWeight.Text);          
            jobN.totalAxleQty = int.Parse(tbTruckAxleQty.Text)+ int.Parse(tbChassisAxleQty.Text);
            //testing
            lbtotalaxleqty.Text = "Total Axle Qty : "+jobN.totalAxleQty.ToString();
            //set authorized payload by total axle quantity
            var authorizedPayloadbyAxle = new Dictionary<double, double>(){
            {3,26000},
            {4,34000},
            {5,42000},
            {6,48000}
            };
            foreach (var kvp in authorizedPayloadbyAxle)
                if (jobN.totalAxleQty == kvp.Key)
                {
                    jobN.authorizedPayload = kvp.Value;
                };       

            //testing
            lbauthorizedpayload.Text = "Authorized Payload : "+ jobN.authorizedPayload.ToString();

            double cntrOverloadThreshold = 0;
            double factor1 = 0;
            double factor2 = 0;
            double factor3 = 0;
            //calculate factor 1 
            factor1 = jobN.authorizedPayload * 1.1 - truckN.truckTareWeight - chassisN.chassisTareWeight - jobN.driverWeight;
            lbfactor1.Text = "Factor 1 : "+ Math.Ceiling(factor1).ToString();
            //calculate factor 2
            factor2 = ((jobN.authorizedPayload * 1.1) + chassisN.chassisAuthorizedPayload - truckN.truckTareWeight - chassisN.chassisTareWeight - jobN.driverWeight) / 2;
            lbfactor2.Text = "Factor 2 : " + Math.Ceiling(factor2).ToString();
            //calculate factor 3
            factor3 = ((jobN.authorizedPayload * 1.1) + truckN.truckAuthorizedPayload - truckN.truckTareWeight - (2 * chassisN.chassisTareWeight) - jobN.driverWeight) / 2;
            lbfactor3.Text = "Factor 3 : " + Math.Ceiling(factor3).ToString();
            double[] factors = { factor1, factor2, factor3 };
            cntrOverloadThreshold = factors.Min();

            tbContainerThreshold.Text = Math.Ceiling(cntrOverloadThreshold).ToString();
            jobN.cntrOverloadThreshold = cntrOverloadThreshold;

            if (Form1.calStep1() == true)
            {
                lbStep1Result.Text = "PASSED";
                lbStep1Result.ForeColor = System.Drawing.Color.Green;
            }
            else {
                lbStep1Result.Text = "FAILED";
                lbStep1Result.ForeColor = System.Drawing.Color.Red;
            }
            if (Form1.calStep2() == true) { lbStep2Result.Text = "PASSED"; 
                lbStep2Result.ForeColor = System.Drawing.Color.Green; 
            } 
            else { 
                lbStep2Result.Text = "FAILED"; 
                lbStep2Result.ForeColor = System.Drawing.Color.Red; 
            }
            if (Form1.calStep3() == true) { 
                lbStep3Result.Text = "PASSED"; 
                lbStep3Result.ForeColor = System.Drawing.Color.Green; 
            } else { 
                lbStep3Result.Text = "FAILED"; 
                lbStep3Result.ForeColor = System.Drawing.Color.Red; 
            }

            //final result display    
           // bool finalValidate = false;
            if (Form1.calStep1() == true && Form1.calStep2() == true && Form1.calStep3() == true && jobN.containersWeight < cntrOverloadThreshold)
            {
              //  finalValidate = true;
                lbFinal.Text = "PASSED";
                lbFinal.ForeColor = System.Drawing.Color.Green;
                jobN.finalresult = true;
            }
            else { 
              //  finalValidate = false; 
                lbFinal.Text = "FAILED";
                lbFinal.ForeColor = System.Drawing.Color.Red;
                jobN.finalresult = false;
            }

            //ver 0.2 se bo sung tinh nang display len grid + luu vao json file
            //dataGridView1.Rows[0].Cells[0].Value = 1;
            //dataGridView1.Rows[0].Cells[1].Value = tbTruckNo.Text;
            //dataGridView1.Rows[0].Cells[2].Value = tbTruckNo.Text;
            //dataGridView1.Rows[0].Cells[3].Value = tbTruckAuthorizedPayload.Text;
            //dataGridView1.Rows[0].Cells[4].Value = tbTruckAxleQty.Text;
            //dataGridView1.Rows[0].Cells[5].Value = tbChassisNo.Text;
            //dataGridView1.Rows[0].Cells[6].Value = tbChassisTareWeight.Text;
            //dataGridView1.Rows[0].Cells[7].Value = tbChassisAuthorizedPayload.Text;
            //dataGridView1.Rows[0].Cells[8].Value = tbChassisAxleQty.Text;
            //dataGridView1.Rows[0].Cells[9].Value = tbContainerThreshold.Text;
        }


    }
}




