using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prediction {

    //Coefficients and formulas sourced from V. R. Morrison, The Physics of Frisbees
    private static readonly double g = -9.81;
    //The acceleration of gravity (m/s^2).
    private static readonly double m = 0.175;
    //The mass of a standard frisbee in kilograms.
    private static readonly double RHO = 1.23;
    //The density of air in kg/m^3.
    private static readonly double AREA = 0.0568;
    //The area of a standard frisbee.
    private static readonly double CL0 = 0.1;
    //The lift coefficient at alpha = 0.
    private static readonly double CLA = 1.4;
    //The lift coefficient dependent on alpha.
    private static readonly double CD0 = 0.08;
    //The drag coefficent at alpha = 0.
    private static readonly double CDA = 2.72;
    //The drag coefficient dependent on alpha.
    private static readonly double ALPHA0 = -4;

    //alpha is the pitch angle of the frisbee
    public List<FrisbeeLocation> simulate3D(double x0, double y0, double z0, double vx0, double vy0, double vz0, double alpha, double deltaT)
    {
        List<FrisbeeLocation> ret = new List<FrisbeeLocation>();
        //Calculating the lift coefficient
        //Formulas provided in V. R. Morrison, The Physics of Frisbees
        //Orig. by S. A. Hummel
        double cl = CL0 + CLA * alpha * Mathf.PI / 180;
        //Drag coefficient
        double cd = CD0 + CDA * Mathf.Pow((float)(alpha - ALPHA0) * Mathf.PI / 180, 2);

        //Initial position z = 0.
        double z = z0;
        //Initial position y = y0.
        double y = y0;

        double x = x0;

        //Initial x velocity vz = vz0.
        double vz = vz0;
        //Initial y velocity vy = vy0.
        double vy = vy0;

        double vx = vx0;

        int i = 0;

        //Frisbee has not yet hit the ground
        while (y>0)
        {
            // Equations 15-17 solved for deltaVy (V. R. Morrison, The Physics of Frisbees)
            // Renamed x-axis in 2D simulation to z-axis in our 3d-coordinates
            double deltavy = (RHO * Mathf.Pow((float)vz, 2) * AREA * cl / 2 / m + g) * deltaT;
            // Equations 12-14
            double deltavz = -RHO * Mathf.Pow((float)vz, 2) * AREA * cd * deltaT;

            // Faking the side-to-side movement due to gyroscopic precession or other unknown effects
            // Inspired by https://discgolf.ultiworld.com/2017/05/02/tuesday-tips-disc-stability-release-angles-work-together/
            double deltavx = 0;

            vz = vz + deltavz;
            vy = vy + deltavy;
            vx = vx + deltavx;

            z = z + vz * deltaT;
            y = y + vy * deltaT;
            x = x + vx * deltaT;

            if (i % 10 == 0)
            {
                //Vector3(x,y,z) x in original 2D simulation
                ret.Add(new FrisbeeLocation(new Vector3((float)x, (float)y, (float)z), (float)vz));
            }
            i++;
        }

        return ret;
    }

}
