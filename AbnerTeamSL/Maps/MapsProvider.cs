using System;
using ESRI.ArcGIS.Client;
using ESRI.ArcGIS.Client.Geometry;
using System.Windows;

namespace AbnerTeamSL.Maps
{
    /// <summary>
    /// 作者：Lucas Abner(苏卜凡)    abnerteam@163.com
    /// 说明：加载天地图在线/离线地图类
    /// 
    ///       支持的地图类型    坐标系    
    /// 在线  矢量/影像/地形    2000/墨卡托(MERCATOR)
    /// 离线  矢量/影像/地形    2000
    /// 
    /// 最近一次更新：2015-07-22 by lucas
    /// </summary>
    public class TianDiTuLayer : TiledMapServiceLayer
    {
        private TianDiTuLayerInfo layerInfo;

        public TianDiTuLayer(int layerType)
            : base()
        {
            this.layerInfo = LayerInfoFactory.getLayerInfo(layerType);
        }

        public override string GetTileUrl(int level, int row, int col)
        {
            if (level > layerInfo.getMaxZoomLevel()
                            || level < layerInfo.getMinZoomLevel())
                return "";
            String url = layerInfo.getUrl()
                            + "?service=wmts&request=gettile&version=1.0.0&layer="
                            + layerInfo.getLayerName() + "&format=tiles&tilematrixset="
                            + layerInfo.getTileMatrixSet() + "&tilecol=" + col
                            + "&tilerow=" + row + "&tilematrix=" + (level + 2);
            return url;
        }

        public override void Initialize()
        {
            this.SpatialReference = new SpatialReference(layerInfo.getSrid());

            this.FullExtent = new Envelope(layerInfo.getxMin(), layerInfo.getyMin(), layerInfo.getxMax(), layerInfo.getyMax());
            //this.FullExtent = new ESRI.ArcGIS.Client.Geometry.Envelope(114.063152040891, 35.7689871732365, 117.550445996933, 37.1752371732365);
            this.TileInfo = new TileInfo()
            {
                Height = layerInfo.getTileHeight(),
                Width = layerInfo.getTileWidth(),

                Origin = layerInfo.getOrigin(),
                Lods = new Lod[layerInfo.getResolutions().Length]
            };
            for (int i = 0; i < TileInfo.Lods.Length; i++)
            {
                TileInfo.Lods[i] = new Lod() { Resolution = layerInfo.getResolutions()[i] };

            }

            // 调用 base initialize to raise the initialization event
            base.Initialize();
        }
    }
    public static class TianDiTuLayerTypes
    {

        /**
		 * 天地图矢量墨卡托投影地图服务
		 */
        public const int TIANDITU_VECTOR_MERCATOR = 0;
        /**
		 * 天地图矢量墨卡托中文标注
		 */
        public const int TIANDITU_VECTOR_ANNOTATION_CHINESE_MERCATOR = 1;
        /**
		 * 天地图矢量墨卡托英文标注
		 */
        public const int TIANDITU_VECTOR_ANNOTATION_ENGLISH_MERCATOR = 2;
        /**
		 * 天地图影像墨卡托投影地图服务
		 */
        public const int TIANDITU_IMAGE_MERCATOR = 3;
        /**
		 * 天地图影像墨卡托投影中文标注
		 */
        public const int TIANDITU_IMAGE_ANNOTATION_CHINESE_MERCATOR = 4;
        /**
		 * 天地图影像墨卡托投影英文标注
		 */
        public const int TIANDITU_IMAGE_ANNOTATION_ENGLISH_MERCATOR = 5;
        /**
		 * 天地图地形墨卡托投影地图服务
		 */
        public const int TIANDITU_TERRAIN_MERCATOR = 6;
        /**
		 * 天地图地形墨卡托投影中文标注
		 */
        public const int TIANDITU_TERRAIN_ANNOTATION_CHINESE_MERCATOR = 7;
        /**
		 * 天地图矢量国家2000坐标系地图服务
		 */
        public const int TIANDITU_VECTOR_2000 = 8;
        /**
		 * 天地图矢量国家2000坐标系中文标注
		 */
        public const int TIANDITU_VECTOR_ANNOTATION_CHINESE_2000 = 9;
        /**
		 * 天地图矢量国家2000坐标系英文标注
		 */
        public const int TIANDITU_VECTOR_ANNOTATION_ENGLISH_2000 = 10;
        /**
		 * 天地图影像国家2000坐标系地图服务
		 */
        public const int TIANDITU_IMAGE_2000 = 11;
        /**
		 * 天地图影像国家2000坐标系中文标注
		 */
        public const int TIANDITU_IMAGE_ANNOTATION_CHINESE_2000 = 12;
        /**
		 * 天地图影像国家2000坐标系英文标注
		 */
        public const int TIANDITU_IMAGE_ANNOTATION_ENGLISH_2000 = 13;
        /**
		 * 天地图地形国家2000坐标系地图服务
		 */
        public const int TIANDITU_TERRAIN_2000 = 14;
        /**
		 * 天地图地形国家2000坐标系中文标注
		 */
        public const int TIANDITU_TERRAIN_ANNOTATION_CHINESE_2000 = 15;
        /**
		 * 离线天地图影像国家2000坐标系
		 */
        public const int TIANDITU_IMAGE_2000_OFFLINE = 16;
        /**
		 * 离线天地图影像国家2000坐标系中文标注
		 */
        public const int TIANDITU_IMAGE_ANNOTATION_CHINESE_2000_OFFLINE = 17;
        /**
		 * 离线天地图矢量国家2000坐标系
		 */
        public const int TIANDITU_VECTOR_2000_OFFLINE = 18;
        /**
		 * 离线天地图矢量国家2000坐标系中文标注
		 */
        public const int TIANDITU_VECTOR_ANNOTATION_CHINESE_2000_OFFLINE = 19;
        /**
		 * 离线天地图矢量国家2000坐标系
		 */
        public const int TIANDITU_TERRAIN_2000_OFFLINE = 20;
        /**
		 * 离线天地图矢量国家2000坐标系中文标注
		 */
        public const int TIANDITU_TERRAIN_ANNOTATION_CHINESE_2000_OFFLINE = 21;
    }
    public class TianDiTuLayerInfo
    {

        private String url;
        private String layerName;

        private int minZoomLevel = 0;
        private int maxZoomLevel = 16;

        private double xMin;
        private double yMin;
        private double xMax;
        private double yMax;

        private int tileWidth = 256;
        private int tileHeight = 256;

        private double[] scales;
        private double[] resolutions;

        private int dpi = 96;

        private int srid;

        private MapPoint origin;

        private String tileMatrixSet;

        public String getUrl()
        {
            return url;
        }

        public void setUrl(String url)
        {
            this.url = url;
        }

        public String getLayerName()
        {
            return layerName;
        }

        public void setLayerName(String layerName)
        {
            this.layerName = layerName;
        }

        public int getMinZoomLevel()
        {
            return minZoomLevel;
        }

        public void setMinZoomLevel(int minZoomLevel)
        {
            this.minZoomLevel = minZoomLevel;
        }

        public int getMaxZoomLevel()
        {
            return maxZoomLevel;
        }

        public void setMaxZoomLevel(int maxZoomLevel)
        {
            this.maxZoomLevel = maxZoomLevel;
        }

        public double getxMin()
        {
            return xMin;
        }

        public void setxMin(double xMin)
        {
            this.xMin = xMin;
        }

        public double getyMin()
        {
            return yMin;
        }

        public void setyMin(double yMin)
        {
            this.yMin = yMin;
        }

        public double getxMax()
        {
            return xMax;
        }

        public void setxMax(double xMax)
        {
            this.xMax = xMax;
        }

        public double getyMax()
        {
            return yMax;
        }

        public void setyMax(double yMax)
        {
            this.yMax = yMax;
        }

        public int getTileWidth()
        {
            return tileWidth;
        }

        public void setTileWidth(int tileWidth)
        {
            this.tileWidth = tileWidth;
        }

        public int getTileHeight()
        {
            return tileHeight;
        }

        public void setTileHeight(int tileHeight)
        {
            this.tileHeight = tileHeight;
        }

        public double[] getScales()
        {
            return scales;
        }

        public void setScales(double[] scales)
        {
            this.scales = scales;
        }

        public double[] getResolutions()
        {
            return resolutions;
        }

        public void setResolutions(double[] resolutions)
        {
            this.resolutions = resolutions;
        }

        public int getDpi()
        {
            return dpi;
        }

        public void setDpi(int dpi)
        {
            this.dpi = dpi;
        }

        public int getSrid()
        {
            return srid;
        }

        public void setSrid(int srid)
        {
            this.srid = srid;
        }

        public MapPoint getOrigin()
        {
            return origin;
        }

        public void setOrigin(MapPoint origin)
        {
            this.origin = origin;
        }

        public String getTileMatrixSet()
        {
            return tileMatrixSet;
        }

        public void setTileMatrixSet(String tileMatrixSet)
        {
            this.tileMatrixSet = tileMatrixSet;
        }

    }
    public class LayerInfoFactory
    {
        private static string UrlString = MapHelper.GetUrl();//获取服务地址 格式: http:XXX.com/
        //国家2000坐标系
        private static String URL_VECTOR_2000 = "http://t0.tianditu.com/vec_c/wmts";//矢量 
        private static String URL_VECTOR_ANNOTATION_CHINESE_2000 = "http://t0.tianditu.com/cva_c/wmts";
        private static String URL_VECTOR_ANNOTATION_ENGLISH_2000 = "http://t0.tianditu.com/eva_c/wmts";
        private static String URL_IMAGE_2000 = "http://t0.tianditu.com/img_c/wmts";//影像
        private static String URL_IMAGE_ANNOTATION_CHINESE_2000 = "http://t0.tianditu.com/cia_c/wmts";
        private static String URL_IMAGE_ANNOTATION_ENGLISH_2000 = "http://t0.tianditu.com/eia_c/wmts";
        private static String URL_TERRAIN_2000 = "http://t0.tianditu.com/ter_c/wmts";//地形
        private static String URL_TERRAIN_ANNOTATION_CHINESE_2000 = "http://t0.tianditu.com/cta_c/wmts";
        private static String URL_VECTOR_2000_OFFLINE = UrlString + "Maps/TianDiTu_Vec";//离线-矢量
        private static String URL_VECTOR_ANNOTATION_CHINESE_2000_OFFLINE = UrlString + "Maps/TianDiTu_Vec_Annotatoin";
        private static String URL_IMAGE_2000_OFFLINE = UrlString + "Maps/TianDiTu_Img";//离线-影像
        private static String URL_IMAGE_ANNOTATION_CHINESE_2000_OFFLINE = UrlString + "Maps/TianDiTu_Img_Annotatoin";
        private static String URL_TERRAIN_2000_OFFLINE = UrlString + "Maps/TianDiTu_Ter";//离线-地形
        private static String URL_TERRAIN_ANNOTATION_CHINESE_2000_OFFLINE = UrlString + "Maps/TianDiTu_Ter_Annotatoin";

        //墨卡托投影
        private static String URL_VECTOR_MERCATOR = "http://t0.tianditu.com/vec_w/wmts";
        private static String URL_VECTOR_ANNOTATION_CHINESE_MERCATOR = "http://t0.tianditu.com/cva_w/wmts";
        private static String URL_VECTOR_ANNOTATION_ENGLISH_MERCATOR = "http://t0.tianditu.com/eva_w/wmts";
        private static String URL_IMAGE_MERCATOR = "http://t0.tianditu.com/img_w/wmts";
        private static String URL_IMAGE_ANNOTATION_CHINESE_MERCATOR = "http://t0.tianditu.com/cia_w/wmts";
        private static String URL_IMAGE_ANNOTATION_ENGLISH_MERCATOR = "http://t0.tianditu.com/eia_w/wmts";
        private static String URL_TERRAIN_MERCATOR = "http://t0.tianditu.com/ter_w/wmts";
        private static String URL_TERRAIN_ANNOTATION_CHINESE_MERCATOR = "http://t0.tianditu.com/cta_w/wmts";

        private static String LAYER_NAME_VECTOR = "vec";
        private static String LAYER_NAME_VECTOR_ANNOTATION_CHINESE = "cva";
        private static String LAYER_NAME_VECTOR_ANNOTATION_ENGLISH = "eva";
        private static String LAYER_NAME_IMAGE = "img";
        private static String LAYER_NAME_IMAGE_ANNOTATION_CHINESE = "cia";
        private static String LAYER_NAME_IMAGE_ANNOTATION_ENGLISH = "eia";
        private static String LAYER_NAME_TERRAIN = "ter";
        private static String LAYER_NAME_TERRAIN_ANNOTATION_CHINESE = "cta";

        private static String LAYER_NAME_OFFLINE = ".jpg";//离线地图底图默认调用 .jpg
        private static String LAYER_NAME_ANNOTATION_CHINESE_OFFLINE = ".png";//离线地图底图默认调用 .png

        private static String TILE_MATRIX_SET_MERCATOR = "w";
        private static String TILE_MATRIX_SET_2000 = "c";

        private static MapPoint ORIGIN_2000 = new MapPoint(-180, 90);//-180,90
                                                                     //private static MapPoint ORIGIN_2000 = new MapPoint(90, 40);
        private static MapPoint ORIGIN_MERCATOR = new MapPoint(-20037508.3427892, 20037508.3427892);
        //private static MapPoint ORIGIN_2000 = new MapPoint(-114,35);
        //114.063152040891, 35.7689871732365, 117.550445996933, 37.1752371732365

        private static int SRID_2000 = 4490;
        private static int SRID_MERCATOR = 102100;

        private static double X_MIN_2000 = -180;
        private static double Y_MIN_2000 = -90;
        private static double X_MAX_2000 = 180;
        private static double Y_MAX_2000 = 90;

        //private static double X_MIN_2000 = 80;
        //private static double Y_MIN_2000 = 20;
        //private static double X_MAX_2000 = 150;
        //private static double Y_MAX_2000 = 40;

        //private static double X_MIN_2000 = 88.3564929352205;
        //private static double Y_MIN_2000 = 18.6758186979439;
        //private static double X_MAX_2000 = 147.682664810221;
        //private static double Y_MAX_2000 = 47.7236702604439;

        private static double X_MIN_MERCATOR = -20037508.3427892;
        private static double Y_MIN_MERCATOR = -20037508.3427892;
        private static double X_MAX_MERCATOR = 20037508.3427892;
        private static double Y_MAX_MERCATOR = 20037508.3427892;

        private static double[] SCALES = { 2.958293554545656E8,
                                                                                 1.479146777272828E8,
                                                                                 7.39573388636414E7,
                                                                                 3.69786694318207E7,
                                                                                 1.848933471591035E7,
                                                                                 9244667.357955175,
                                                                                 4622333.678977588,
                                                                                 2311166.839488794,
                                                                                 1155583.419744397,
                                                                                 577791.7098721985,
                                                                                 288895.85493609926,
                                                                                 144447.92746804963,
                                                                                 72223.96373402482,
                                                                                 36111.98186701241,
                                                                                 18055.990933506204,
                                                                                 9027.995466753102,
                                                                                 4513.997733376551,
                                                                                 2256.998866688275 };

        private static double[] RESOLUTIONS_2000 = {
        0.3515625, 0.17578125,0.087890625, 0.0439453125, 0.02197265625, 0.010986328125,
            0.0054931640625, 0.00274658203125, 0.001373291015625,
            0.0006866455078125, 0.00034332275390625, 0.000171661376953125,
            8.58306884765625E-05, 4.29153442382813E-05, 2.14576721191406E-05 };
        private static double[] RESOLUTIONS_MERCATOR = { 39135.83675440267,
            19567.918377201335, 9783.959188600667, 4891.979594300334,
            2445.989797150167, 1222.9948985750834, 611.4974492875417,
            305.7487246437696, 152.87436232188531, 76.43718116094266,
            38.21859058047133, 19.109295290235693, 9.554647645117846,
            4.777323822558923, 2.3886619112794585, 1.1943309556397292,
            0.597165477819866 };

        public static TianDiTuLayerInfo getLayerInfo(int layerType)
        {
            TianDiTuLayerInfo layerInfo = new TianDiTuLayerInfo();
            switch (layerType)
            {
                case TianDiTuLayerTypes.TIANDITU_IMAGE_2000:

                    layerInfo.setUrl(LayerInfoFactory.URL_IMAGE_2000);
                    layerInfo.setLayerName(LayerInfoFactory.LAYER_NAME_IMAGE);
                    break;
                case TianDiTuLayerTypes.TIANDITU_IMAGE_ANNOTATION_CHINESE_2000:
                    layerInfo.setUrl(LayerInfoFactory.URL_IMAGE_ANNOTATION_CHINESE_2000);
                    layerInfo.setLayerName(LayerInfoFactory.LAYER_NAME_IMAGE_ANNOTATION_CHINESE);
                    break;
                case TianDiTuLayerTypes.TIANDITU_IMAGE_ANNOTATION_ENGLISH_2000:
                    layerInfo.setUrl(LayerInfoFactory.URL_IMAGE_ANNOTATION_ENGLISH_2000);
                    layerInfo.setLayerName(LayerInfoFactory.LAYER_NAME_IMAGE_ANNOTATION_ENGLISH);
                    break;
                case TianDiTuLayerTypes.TIANDITU_IMAGE_ANNOTATION_CHINESE_MERCATOR:
                    layerInfo.setUrl(LayerInfoFactory.URL_IMAGE_ANNOTATION_CHINESE_MERCATOR);
                    layerInfo.setLayerName(LayerInfoFactory.LAYER_NAME_IMAGE_ANNOTATION_CHINESE);
                    break;
                case TianDiTuLayerTypes.TIANDITU_IMAGE_ANNOTATION_ENGLISH_MERCATOR:
                    layerInfo.setUrl(LayerInfoFactory.URL_IMAGE_ANNOTATION_ENGLISH_MERCATOR);
                    layerInfo.setLayerName(LayerInfoFactory.LAYER_NAME_IMAGE_ANNOTATION_ENGLISH);
                    break;
                case TianDiTuLayerTypes.TIANDITU_IMAGE_MERCATOR:
                    layerInfo.setUrl(LayerInfoFactory.URL_IMAGE_MERCATOR);
                    layerInfo.setLayerName(LayerInfoFactory.LAYER_NAME_IMAGE);
                    break;
                case TianDiTuLayerTypes.TIANDITU_VECTOR_2000:

                    layerInfo.setUrl(LayerInfoFactory.URL_VECTOR_2000);
                    layerInfo.setLayerName(LayerInfoFactory.LAYER_NAME_VECTOR);
                    break;
                case TianDiTuLayerTypes.TIANDITU_VECTOR_ANNOTATION_CHINESE_2000:
                    layerInfo.setUrl(LayerInfoFactory.URL_VECTOR_ANNOTATION_CHINESE_2000);
                    layerInfo.setLayerName(LayerInfoFactory.LAYER_NAME_VECTOR_ANNOTATION_CHINESE);
                    break;
                case TianDiTuLayerTypes.TIANDITU_VECTOR_ANNOTATION_ENGLISH_2000:
                    layerInfo.setUrl(LayerInfoFactory.URL_VECTOR_ANNOTATION_ENGLISH_2000);
                    layerInfo.setLayerName(LayerInfoFactory.LAYER_NAME_VECTOR_ANNOTATION_ENGLISH);
                    break;
                case TianDiTuLayerTypes.TIANDITU_VECTOR_ANNOTATION_CHINESE_MERCATOR:
                    layerInfo.setUrl(LayerInfoFactory.URL_VECTOR_ANNOTATION_CHINESE_MERCATOR);
                    layerInfo.setLayerName(LayerInfoFactory.LAYER_NAME_VECTOR_ANNOTATION_CHINESE);
                    break;
                case TianDiTuLayerTypes.TIANDITU_VECTOR_ANNOTATION_ENGLISH_MERCATOR:
                    layerInfo.setUrl(LayerInfoFactory.URL_VECTOR_ANNOTATION_ENGLISH_MERCATOR);
                    break;
                case TianDiTuLayerTypes.TIANDITU_VECTOR_MERCATOR:
                    layerInfo.setUrl(LayerInfoFactory.URL_VECTOR_MERCATOR);
                    layerInfo.setLayerName(LayerInfoFactory.LAYER_NAME_VECTOR);
                    break;
                case TianDiTuLayerTypes.TIANDITU_TERRAIN_2000:
                    layerInfo.setUrl(LayerInfoFactory.URL_TERRAIN_2000);
                    layerInfo.setLayerName(LayerInfoFactory.LAYER_NAME_TERRAIN);
                    break;
                case TianDiTuLayerTypes.TIANDITU_TERRAIN_ANNOTATION_CHINESE_2000:
                    layerInfo.setUrl(LayerInfoFactory.URL_TERRAIN_ANNOTATION_CHINESE_2000);
                    layerInfo.setLayerName(LayerInfoFactory.LAYER_NAME_TERRAIN_ANNOTATION_CHINESE);
                    break;
                case TianDiTuLayerTypes.TIANDITU_TERRAIN_MERCATOR:
                    layerInfo.setUrl(LayerInfoFactory.URL_TERRAIN_MERCATOR);
                    layerInfo.setLayerName(LayerInfoFactory.LAYER_NAME_TERRAIN);
                    break;
                case TianDiTuLayerTypes.TIANDITU_TERRAIN_ANNOTATION_CHINESE_MERCATOR:
                    layerInfo.setUrl(LayerInfoFactory.URL_TERRAIN_ANNOTATION_CHINESE_MERCATOR);
                    layerInfo.setLayerName(LayerInfoFactory.LAYER_NAME_TERRAIN_ANNOTATION_CHINESE);
                    break;
                case TianDiTuLayerTypes.TIANDITU_IMAGE_2000_OFFLINE:
                    layerInfo.setUrl(LayerInfoFactory.URL_IMAGE_2000_OFFLINE);
                    layerInfo.setLayerName(LayerInfoFactory.LAYER_NAME_OFFLINE);
                    break;
                case TianDiTuLayerTypes.TIANDITU_IMAGE_ANNOTATION_CHINESE_2000_OFFLINE:
                    layerInfo.setUrl(LayerInfoFactory.URL_IMAGE_ANNOTATION_CHINESE_2000_OFFLINE);
                    layerInfo.setLayerName(LayerInfoFactory.LAYER_NAME_ANNOTATION_CHINESE_OFFLINE);
                    break;
                case TianDiTuLayerTypes.TIANDITU_VECTOR_2000_OFFLINE:
                    layerInfo.setUrl(LayerInfoFactory.URL_VECTOR_2000_OFFLINE);
                    layerInfo.setLayerName(LayerInfoFactory.LAYER_NAME_OFFLINE);
                    break;
                case TianDiTuLayerTypes.TIANDITU_VECTOR_ANNOTATION_CHINESE_2000_OFFLINE:
                    layerInfo.setUrl(LayerInfoFactory.URL_VECTOR_ANNOTATION_CHINESE_2000_OFFLINE);
                    layerInfo.setLayerName(LayerInfoFactory.LAYER_NAME_ANNOTATION_CHINESE_OFFLINE);
                    break;
                case TianDiTuLayerTypes.TIANDITU_TERRAIN_2000_OFFLINE:
                    layerInfo.setUrl(LayerInfoFactory.URL_TERRAIN_2000_OFFLINE);
                    layerInfo.setLayerName(LayerInfoFactory.LAYER_NAME_OFFLINE);
                    break;
                case TianDiTuLayerTypes.TIANDITU_TERRAIN_ANNOTATION_CHINESE_2000_OFFLINE:
                    layerInfo.setUrl(LayerInfoFactory.URL_TERRAIN_ANNOTATION_CHINESE_2000);
                    layerInfo.setLayerName(LayerInfoFactory.LAYER_NAME_ANNOTATION_CHINESE_OFFLINE);
                    break;
            }
            handleLayerInfo(layerInfo, layerType);
            return layerInfo;
        }

        private static void handleLayerInfo(TianDiTuLayerInfo layerInfo, int layerType)
        {
            switch (layerType)
            {
                case TianDiTuLayerTypes.TIANDITU_IMAGE_2000:
                case TianDiTuLayerTypes.TIANDITU_IMAGE_ANNOTATION_CHINESE_2000:
                case TianDiTuLayerTypes.TIANDITU_IMAGE_ANNOTATION_ENGLISH_2000:
                case TianDiTuLayerTypes.TIANDITU_VECTOR_2000:
                case TianDiTuLayerTypes.TIANDITU_VECTOR_ANNOTATION_CHINESE_2000:
                case TianDiTuLayerTypes.TIANDITU_VECTOR_ANNOTATION_ENGLISH_2000:
                case TianDiTuLayerTypes.TIANDITU_TERRAIN_2000:
                case TianDiTuLayerTypes.TIANDITU_TERRAIN_ANNOTATION_CHINESE_2000:
                case TianDiTuLayerTypes.TIANDITU_IMAGE_2000_OFFLINE:
                case TianDiTuLayerTypes.TIANDITU_IMAGE_ANNOTATION_CHINESE_2000_OFFLINE:
                case TianDiTuLayerTypes.TIANDITU_VECTOR_2000_OFFLINE:
                case TianDiTuLayerTypes.TIANDITU_VECTOR_ANNOTATION_CHINESE_2000_OFFLINE:
                case TianDiTuLayerTypes.TIANDITU_TERRAIN_2000_OFFLINE:
                case TianDiTuLayerTypes.TIANDITU_TERRAIN_ANNOTATION_CHINESE_2000_OFFLINE:
                    layerInfo.setOrigin(LayerInfoFactory.ORIGIN_2000);
                    layerInfo.setSrid(LayerInfoFactory.SRID_2000);
                    layerInfo.setxMin(LayerInfoFactory.X_MIN_2000);
                    layerInfo.setyMin(LayerInfoFactory.Y_MIN_2000);
                    layerInfo.setxMax(LayerInfoFactory.X_MAX_2000);
                    layerInfo.setyMax(LayerInfoFactory.Y_MAX_2000);
                    layerInfo.setScales(LayerInfoFactory.SCALES);
                    layerInfo.setResolutions(LayerInfoFactory.RESOLUTIONS_2000);
                    layerInfo.setTileMatrixSet(LayerInfoFactory.TILE_MATRIX_SET_2000);
                    break;
                case TianDiTuLayerTypes.TIANDITU_IMAGE_ANNOTATION_CHINESE_MERCATOR:
                case TianDiTuLayerTypes.TIANDITU_IMAGE_ANNOTATION_ENGLISH_MERCATOR:
                case TianDiTuLayerTypes.TIANDITU_IMAGE_MERCATOR:
                case TianDiTuLayerTypes.TIANDITU_VECTOR_ANNOTATION_CHINESE_MERCATOR:
                case TianDiTuLayerTypes.TIANDITU_VECTOR_ANNOTATION_ENGLISH_MERCATOR:
                case TianDiTuLayerTypes.TIANDITU_VECTOR_MERCATOR:
                case TianDiTuLayerTypes.TIANDITU_TERRAIN_MERCATOR:
                case TianDiTuLayerTypes.TIANDITU_TERRAIN_ANNOTATION_CHINESE_MERCATOR:
                    layerInfo.setOrigin(LayerInfoFactory.ORIGIN_MERCATOR);
                    layerInfo.setSrid(LayerInfoFactory.SRID_MERCATOR);
                    layerInfo.setxMin(LayerInfoFactory.X_MIN_MERCATOR);
                    layerInfo.setyMin(LayerInfoFactory.Y_MIN_MERCATOR);
                    layerInfo.setxMax(LayerInfoFactory.X_MAX_MERCATOR);
                    layerInfo.setyMax(LayerInfoFactory.Y_MAX_MERCATOR);
                    layerInfo.setScales(LayerInfoFactory.SCALES);
                    layerInfo.setResolutions(LayerInfoFactory.RESOLUTIONS_MERCATOR);
                    layerInfo.setTileMatrixSet(LayerInfoFactory.TILE_MATRIX_SET_MERCATOR);
                    break;
            }
        }
    }
    public class TianDiTuOffLineLayer : TiledMapServiceLayer
    {
        private TianDiTuLayerInfo layerInfo;
        public TianDiTuOffLineLayer(int layerType): base()
        {
            this.layerInfo = LayerInfoFactory.getLayerInfo(layerType);
        }
        /// <summary>
        /// 离线地图类
        /// 服务器文件夹路径：
        /// 矢量：Map/TianDiTu_Vec
        /// 矢量注记：Map/TianDiTu_Vec_Annotatoin
        /// 影像：Map/TianDiTu_Img
        /// 影像注记：Map/TianDiTu_Img_Annotatoin
        /// 地形：Map/TianDiTu_Ter
        /// 地形注记：Map/TianDiTu_Ter_Annotatoin
        /// </summary>
        /// 
        /// <param name="level">metrix{编号}</param>
        /// <param name="row">row{编号}</param>
        /// <param name="col">col{编号}</param>
        /// <returns></returns>
        public override string GetTileUrl(int level, int row, int col)
        {
            if (level > layerInfo.getMaxZoomLevel()
                            || level < layerInfo.getMinZoomLevel() || layerInfo.getUrl().Contains("tianditu.com"))//离线屏蔽对外请求
                return "";

            String url = layerInfo.getUrl()
                            + "/metrix" + (level + 2) + "/row" + row
                            + "/col" + col + layerInfo.getLayerName();
            return url;
        }

        public override void Initialize()
        {
            this.SpatialReference = new SpatialReference(layerInfo.getSrid());

            this.FullExtent = new Envelope(layerInfo.getxMin(), layerInfo.getyMin(), layerInfo.getxMax(), layerInfo.getyMax());
            //this.FullExtent = new ESRI.ArcGIS.Client.Geometry.Envelope(114.063152040891, 35.7689871732365, 117.550445996933, 37.1752371732365);
            this.TileInfo = new TileInfo()
            {
                Height = layerInfo.getTileHeight(),
                Width = layerInfo.getTileWidth(),

                Origin = layerInfo.getOrigin(),
                Lods = new Lod[layerInfo.getResolutions().Length]
            };
            for (int i = 0; i < TileInfo.Lods.Length; i++)
            {
                TileInfo.Lods[i] = new Lod() { Resolution = layerInfo.getResolutions()[i] };

            }

            // 调用 base initialize to raise the initialization event
            base.Initialize();
        }
    }
    /// <summary>
    /// Map帮助类
    /// </summary>
    public class MapHelper
    {
        /// <summary>
        /// 根据图层名称获取图层Index
        /// </summary>
        /// <param name="_layerName">图层名</param>
        /// <param name="_map">地图LayerCollection</param>
        /// <returns>index/null</returns>
        public static int? GetLayerIndex(string _layerName, LayerCollection _layers)
        {
            int i;
            for (i = 0; i < _layers.Count; i++)
            {
                if (_layers[i].ID == _layerName)
                {
                    return i;
                }
            }
            return null;
        }

        public static string GetUrl()
        {
            string url = Application.Current.Host.Source.ToString();
            return url.Substring(0, url.IndexOf('/', 7) + 1);
        }
    }
}
