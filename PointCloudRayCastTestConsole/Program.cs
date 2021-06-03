using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

using System.Runtime.InteropServices;

namespace PointCloudRayCastTestConsole
{
    class Program
    {
        [DllImport("LIBRAYCASTPOINTPCL18", CallingConvention = CallingConvention.Cdecl)]
        extern static void loadCloud(string file, double[] ray, float camera_fov = 90, float camera_aspect = 1, bool z_up = true);

        [DllImport("LIBRAYCASTPOINTPCL18", CallingConvention = CallingConvention.Cdecl)]
        extern static void get_cloud_extent(double[] extent);

        [DllImport("LIBRAYCASTPOINTPCL18", CallingConvention = CallingConvention.Cdecl)]
        extern static void set_pivot_to_extent_min();

        [DllImport("LIBRAYCASTPOINTPCL18", CallingConvention = CallingConvention.Cdecl)]
        extern static int generate_segment();

        [DllImport("LIBRAYCASTPOINTPCL18", CallingConvention = CallingConvention.Cdecl)]
        extern static void free_cloud_data();

        [DllImport("LIBRAYCASTPOINTPCL18", CallingConvention = CallingConvention.Cdecl)]
        extern static int num_segment(); //returns number of generated segment

        [DllImport("LIBRAYCASTPOINTPCL18", CallingConvention = CallingConvention.Cdecl)]
        extern static int num_points(); //returns number of points in generated segment

        [DllImport("LIBRAYCASTPOINTPCL18", CallingConvention = CallingConvention.Cdecl)]
        extern static int size_planecoff();

        [DllImport("LIBRAYCASTPOINTPCL18", CallingConvention = CallingConvention.Cdecl)]
        extern static void list_points_size(int[] sizes); //returns array of point size for each segment

        [DllImport("LIBRAYCASTPOINTPCL18", CallingConvention = CallingConvention.Cdecl)]
        extern static void list_points(float[] points); //returns array of points for each segment

        [DllImport("LIBRAYCASTPOINTPCL18", CallingConvention = CallingConvention.Cdecl)]
        extern static void list_normals(float[] normals); //returns array of normals for each segment

        [DllImport("LIBRAYCASTPOINTPCL18", CallingConvention = CallingConvention.Cdecl)]
        extern static void list_planecoff(float[] planecoff); //returns array of plane param for each segment

        [DllImport("LIBRAYCASTPOINTPCL18", CallingConvention = CallingConvention.Cdecl)]
        extern static void list_colors(float[] colors); //returns array of colors for each segment


        static void Main(string[] args)
        {
            float[] colors = null;
            float[] points = null;
            float[] plane_coffs = null;
            int[] sizes;
            int n_segment;

            Console.Write("Enter ray coordinates relative to point cloud minimum origin  : \n");

            Console.Write("Enter RayStart X :");
            string strRayStartX = Console.ReadLine();

            Console.Write("Enter RayStart Y :");
            string strRayStartY = Console.ReadLine();

            Console.Write("Enter RayStart Z :");
            string strRayStartZ = Console.ReadLine();

            Console.Write("Enter RayEnd X :");
            string strRayEndX = Console.ReadLine();

            Console.Write("Enter RayEnd Y :");
            string strRayEndY = Console.ReadLine();

            Console.Write("Enter RayEnd Z :");
            string strRayEndZ = Console.ReadLine();

            //gets set dynammiccly with get_cloud_extend
            double cloud_min_x = 131396;
            double cloud_min_y = 398750;
            double cloud_min_z = 12.99;

            //Transform Unity Camera to point cloud space, considering Z up
            double ray_start_x = cloud_min_x + double.Parse(strRayStartX);// cam_pos.x;
            double ray_start_y = cloud_min_y + double.Parse(strRayStartZ);// cam_pos.z;
            double ray_start_z = cloud_min_z + double.Parse(strRayStartY);// cam_pos.y;

            double ray_end_x = cloud_min_x + double.Parse(strRayEndX);// ray_end.x;
            double ray_end_y = cloud_min_y + double.Parse(strRayEndZ);// ray_end.z;
            double ray_end_z = cloud_min_z + double.Parse(strRayEndY);// ray_end.y;


            double[] ray = { ray_start_x, ray_start_y, ray_start_z, ray_end_x, ray_end_y, ray_end_z };


            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            loadCloud("D:\\out\\Test.las", ray, 25);

            stopWatch.Stop();//timeing how long it takes to load the pointcloud in
            Console.Write("Time Stamp End : {0} \n", stopWatch.ElapsedMilliseconds);

            set_pivot_to_extent_min();

            double[] extent = new double[6];
            get_cloud_extent(extent);


            Console.Write("extent min x {0} \n" , extent[0]);
            Console.Write("extent min y {0} \n", extent[1]);
            Console.Write("extent min z {0} \n", extent[2]);

            Console.Write("extent max x {0} \n", extent[3]);
            Console.Write("extent max y {0} ", extent[4]);
            Console.Write("extent max z {0} ", extent[5]);


            //generate_segment();

            n_segment = num_segment();
            Console.Write("n_segment: \n" + n_segment);

            sizes = new int[n_segment];
            list_points_size(sizes);


            int npoint = num_points();
            points = new float[npoint];
            colors = new float[npoint];

            list_points(points);
            list_colors(colors);

            Console.Write("npoints:" + sizes[0]);


            /*//size of plane coff nsegment * sizeof_planecoff
            int sizeof_planecoff = size_planecoff() * n_segment;
            plane_coffs = new float[sizeof_planecoff];
            list_planecoff(plane_coffs);*/



            Console.ReadKey();

        }
    }
}
