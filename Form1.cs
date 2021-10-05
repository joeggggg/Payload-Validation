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

        public void button1_Click(object sender, EventArgs e)
        {
            //need to add validation for input value
            truck truckN = new truck();
          // truckN.truckNo = tbTruckNo.ToString();
            truckN.truckTareWeight = int.Parse(tbTruckTareWeight.Text);
            truckN.truckAllowedMaxWeight = int.Parse(tbTruckAuthorizedPayload.Text);
            truckN.truckAxle = int.Parse(tbTruckAxleQty.Text);

            chassis chassisN = new chassis();
          // chassisN.chassisNo = tbChassisNo.ToString();
            chassisN.chassisTareWeight = int.Parse(tbChassisTareWeight.Text);
            chassisN.chassisAllowedMaxWeight = int.Parse(tbChassisAuthorizedPayload.Text);
            chassisN.chassisAxle = int.Parse(tbChassisAxleQty.Text);

            job jobN = new job();
            jobN.containersWeight = double.Parse(tbContainerWeightSum.Text); 
            jobN.driverWeight = int.Parse(tbDriverWeight.Text);          
            jobN.totalAxleQty = int.Parse(tbTruckAxleQty.Text) + int.Parse(tbChassisAxleQty.Text);
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

            //Step 1:
            // Validate if Container weight< (Truck’s Allowed Max Weight - Chassis’s Tare weight – 65) * 110%
            bool calStep1_isvalid = false;
            if (jobN.containersWeight < (truckN.truckAllowedMaxWeight - chassisN.chassisTareWeight - jobN.driverWeight) * 1.1)
            {
                calStep1_isvalid = true;
            }
            else
            {
                calStep1_isvalid = false;
            }
            if (calStep1_isvalid == true)
            {
                lbStep1Result.Text = "PASSED";
                lbStep1Result.ForeColor = System.Drawing.Color.Green;
            }
            else
            {
                lbStep1Result.Text = "FAILED";
                lbStep1Result.ForeColor = System.Drawing.Color.Red;
            } 

            //Step 2:
            //Validate if Container weight<Chassis’s allowed max weight * 110%
            bool calStep2_isvalid = false;
            if (jobN.containersWeight < (chassisN.chassisAllowedMaxWeight) * 1.1)
            {
                calStep2_isvalid = true;
            }
            else
            {
                calStep2_isvalid = false;
            }

            if (calStep2_isvalid == true)
            {
                lbStep2Result.Text = "PASSED";
                lbStep2Result.ForeColor = System.Drawing.Color.Green;
            }
            else
            {
                lbStep2Result.Text = "FAILED";
                lbStep2Result.ForeColor = System.Drawing.Color.Red;
            }

            //Step 3:
            //•	If total number of Axle = 3, then validate if (Container weight + Truck’s Tare weight + Chassis Tare weight) < 26000 * 110%
            //•	If total number of Axle = 4, then validate if (Container weight + Truck’s Tare weight + Chassis Tare weight) < 34000 * 110%
            //•	If total number of Axle = 5, then validate if (Container weight + Truck’s Tare weight + Chassis Tare weight) < 42000 * 110%
            //•	If total number of Axle = 6, then validate if (Container weight + Truck’s Tare weight + Chassis Tare weight) < 48000 * 110% 

            bool calStep3_isvalid = false;
            if ((jobN.containersWeight + truckN.truckTareWeight + chassisN.chassisTareWeight) < jobN.authorizedPayload * 1.1)
            {
                calStep3_isvalid = true;
            }
            else
            {
                calStep3_isvalid = false;
            }

            if (calStep3_isvalid == true) { 
                lbStep3Result.Text = "PASSED"; 
                lbStep3Result.ForeColor = System.Drawing.Color.Green; 
            } else { 
                lbStep3Result.Text = "FAILED"; 
                lbStep3Result.ForeColor = System.Drawing.Color.Red; 
            }

            //final result display    
            if (calStep1_isvalid == true && calStep2_isvalid == true && calStep3_isvalid == true)
            {
                lbFinal.Text = "PASSED";
                lbFinal.ForeColor = System.Drawing.Color.Green;
                jobN.finalresult = true;
            }
            else { 
                lbFinal.Text = "FAILED";
                lbFinal.ForeColor = System.Drawing.Color.Red;
                jobN.finalresult = false;
            }
         }

        private void btnReset_Click(object sender, EventArgs e)
        {
            tbTruckTareWeight.Text = null;
            tbTruckAuthorizedPayload.Text = null;
            tbTruckTareWeight.Text = null;
            tbTruckAxleQty.Text = null;
            tbChassisTareWeight.Text = null;
            tbChassisAuthorizedPayload.Text = null;
            tbChassisAxleQty.Text = null;
            tbContainerWeightSum.Text = null;
            lbStep1Result.Text = null;
            lbStep2Result.Text = null;
            lbStep3Result.Text = null;
            lbFinal.Text = null;

        }
    }
}




