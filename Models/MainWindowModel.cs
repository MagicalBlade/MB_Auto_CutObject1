using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Tekla.Structures;
using Tekla.Structures.Datatype;
using Tekla.Structures.Geometry3d;
using Tekla.Structures.Model;
using Tekla.Structures.Model.Operations;
using Tekla.Structures.Model.UI;
using Tekla.Structures.Plugins;
using Tekla.Structures.Solid;
using static MB_Auto_CutObject1.Views.MainWindow;
using TSG = Tekla.Structures.Geometry3d;

namespace MB_Auto_CutObject1.Models
{
    public class PluginData
    {
        #region Fields
        //
        // Define the fields specified on the Form.
        //
        [StructuresField("height")]
        public double height;
        [StructuresField("height1")]
        public double height1;
        [StructuresField("width")]
        public double width;
        [StructuresField("width1")]
        public double width1;
        [StructuresField("width2")]
        public double width2;
        [StructuresField("width3")]
        public double width3;
        [StructuresField("width4")]
        public double width4;
        [StructuresField("radius")]
        public double radius;
        [StructuresField("offsetH")]
        public double offsetH;
        [StructuresField("offsetL")]
        public double offsetL;
        [StructuresField("dimensionF1")]
        public double dimensionF1;
        [StructuresField("dimensionF2")]
        public double dimensionF2;
        [StructuresField("dimensionF3")]
        public double dimensionF3;
        [StructuresField("typeCut")]
        public int typeCut;
        [StructuresField("mirror")]
        public int mirror;
        [StructuresField("typeChamfer")]
        public int typeChamfer;
        #endregion


    }

    [Plugin("Мостостроение.Авто. Выкружки и вырезы.")]
    [PluginUserInterface("MB_Auto_CutObject1.Views.MainWindow")]
    [InputObjectDependency(InputObjectDependency.DEPENDENT)]
    public class MB_Auto_CutObject1 : PluginBase
    {
        #region Fields
        private Model _Model;
        private PluginData _Data;
        //
        // Define variables for the field values.
        //
        private double _Height = 0.0;
        private double _Height1 = 0.0;
        private double _Width = 0.0;
        private double _Width1 = 0.0;
        private double _Width2 = 0.0;
        private double _Width3 = 0.0;
        private double _Width4 = 0.0;
        private double _Radius = 0.0;
        private double _OffsetH = 0.0;
        private double _OffsetL = 0.0;
        private double _DimensionF1 = 0.0;
        private double _DimensionF2 = 0.0;
        private double _DimensionF3 = 0.0;
        private int _TypeCut = 0;
        private int _Mirror = 0;
        private int _TypeChamfer = 0;

        #endregion

        #region Properties
        private Model Model
        {
            get { return this._Model; }
            set { this._Model = value; }
        }

        private PluginData Data
        {
            get { return this._Data; }
            set { this._Data = value; }
        }
        #endregion

        #region Constructor
        public MB_Auto_CutObject1(PluginData data)
        {
            Model = new Model();
            Data = data;
        }
        #endregion


        /// <summary>
        /// Запрос к пользователю
        /// </summary>
        /// <returns></returns>

        public override List<InputDefinition> DefineInput()
        {
            List<InputDefinition> Input = new List<InputDefinition>();
            Picker Picker = new Picker();
            Part part = (Part)Picker.PickObject(Picker.PickObjectEnum.PICK_ONE_PART);
            Part part1 = (Part)Picker.PickObject(Picker.PickObjectEnum.PICK_ONE_PART);

            Input.Add(new InputDefinition(part.Identifier));
            Input.Add(new InputDefinition(part1.Identifier));

            return Input;
        }

        /// <summary>
        /// Тело библиотеки
        /// </summary>
        /// <param name="Input"></param>
        /// <returns></returns>
        public override bool Run(List<InputDefinition> Input)
        {
            try
            {
                GetValuesFromDialog();
                if (Input == null)
                {
                    return false;
                }
                Part selectedPart = (Part)Model.SelectModelObject((Identifier)Input[0].GetInput());
                Part selectedPart1 = (Part)Model.SelectModelObject((Identifier)Input[1].GetInput());
                Solid solidpart = selectedPart.GetSolid();
                Solid solidpart1 = selectedPart1.GetSolid();
                //Получаем толщину продольного ребра
                WorkPlaneHandler workPlaneHandler = Model.GetWorkPlaneHandler();
                TransformationPlane tpcurrent = workPlaneHandler.GetCurrentTransformationPlane();
                TransformationPlane tppart = new TransformationPlane(selectedPart.GetCoordinateSystem());
                TransformationPlane tppart1 = new TransformationPlane(selectedPart1.GetCoordinateSystem());

                //workPlaneHandler.SetCurrentTransformationPlane(tppart1);
                //_Width1 = Math.Round(solidpart1.MaximumPoint.Z - solidpart1.MinimumPoint.Z);
                //workPlaneHandler.SetCurrentTransformationPlane(tppart);
                

                
                Plane plane = new Plane
                {
                    Origin = selectedPart.GetCoordinateSystem().Origin,
                    AxisX = selectedPart.GetCoordinateSystem().AxisX,
                    AxisY = selectedPart.GetCoordinateSystem().AxisY
                };
                ControlPlane controlPlane = new ControlPlane(plane, false);
                controlPlane.Insert();

                Plane plane1 = new Plane
                {
                    Origin = selectedPart1.GetCoordinateSystem().Origin,
                    AxisX = selectedPart1.GetCoordinateSystem().AxisX,
                    AxisY = selectedPart1.GetCoordinateSystem().AxisY
                };
                ControlPlane controlPlane1 = new ControlPlane(plane1, false);
                controlPlane1.Insert();
                
                GeometricPlane geometricPlane = new GeometricPlane(selectedPart.GetCoordinateSystem());
                GeometricPlane geometricPlane1 = new GeometricPlane(selectedPart1.GetCoordinateSystem());
                
                Line line = Intersection.PlaneToPlane(geometricPlane, geometricPlane1);
                
                ControlPoint cpOrigin = new ControlPoint(line.Origin);
                cpOrigin.Insert();

                ControlPoint cpDirection = new ControlPoint(line.Direction);
                cpDirection.Insert();

                ControlLine controlLine = new ControlLine();
                controlLine.Line = new LineSegment(line.Origin, line.Direction);
                
                controlLine.Insert();


                FaceEnumerator faceEnum = solidpart1.GetFaceEnumerator();
                while (faceEnum.MoveNext())
                {
                    Face face = faceEnum.Current;
                }
                
                /*
                ContourPoint selectedpoint1 = new ContourPoint(new TSG.Point(line.Origin.X, line.Origin.Y, line.Origin.Z), null);
                ContourPoint selectedpoint2 = new ContourPoint(new TSG.Point(line.Origin.X + 10, line.Origin.Y, line.Origin.Z), null);

                double centerpart = (solidpart.MaximumPoint.Z + solidpart.MinimumPoint.Z) / 2;

                ContourPlate booleanCP = new ContourPlate();
                double thicknessCut = Math.Round(solidpart.MaximumPoint.Z - solidpart.MinimumPoint.Z) + 10;
                booleanCP.Profile.ProfileString = $"PL{thicknessCut}";
                booleanCP.Material.MaterialString = "Steel_Undefined";
                // В зависимости от выбранного типа выреза добавляем точки контурной пластины
                // Вырез центрируем относитально детали с помощью centerpart
                switch (_TypeCut)
                {
                    case 0:
                        {
                            AddContourPoint(0 - _OffsetL, 0 - _OffsetH, centerpart, booleanCP, null);
                            AddContourPoint(0 - _OffsetL, _Height, centerpart, booleanCP, null);
                            AddContourPoint(_Width, _Height, centerpart, booleanCP, new Chamfer(_Radius, 0, Chamfer.ChamferTypeEnum.CHAMFER_ROUNDING));
                            AddContourPoint(_Width, -_OffsetH, centerpart, booleanCP, null);
                        }
                        break;
                    case 1:
                        {
                            AddContourPoint(0 + _Width + _Radius, _Height, centerpart, booleanCP, new Chamfer(_Radius, 0, Chamfer.ChamferTypeEnum.CHAMFER_ROUNDING));
                            AddContourPoint(0 + _Width - _Width1, _Height, centerpart, booleanCP, null);
                            AddContourPoint(0 + _Width - _Width1, 0 - _OffsetH, centerpart, booleanCP, null);
                            AddContourPoint(0 - _OffsetL, 0 - _OffsetH, centerpart, booleanCP, null);
                            AddContourPoint(0 - _OffsetL, _Height + 2 * _Radius, centerpart, booleanCP, null);
                            AddContourPoint(_Width + _Radius, _Height + 2 * _Radius, centerpart, booleanCP, new Chamfer(_Radius, 0, Chamfer.ChamferTypeEnum.CHAMFER_ROUNDING));
                        }
                        break;
                    case 2:
                        {
                            //Подготовка данных для получения точки касательной к окружности
                            double hyp = Math.Sqrt(Math.Pow(_Width + _Width1 / 2, 2) + Math.Pow(_Height + _Height1, 2));
                            double angle1 = Math.Acos(_Radius / hyp);
                            double angle2 = Math.Acos((_Height + _Height1) / hyp);
                            double angle3 = (angle1 + angle2) - 90 * Math.PI / 180;
                            double hkat = _Radius * Math.Cos(angle3);
                            double vkat = _Radius * Math.Sin(angle3);
                            //Координата Х для удлинения выреза
                            double offsetX = _OffsetH * Math.Tan(angle3);

                            AddContourPoint(0 - _Width - _Width1 / 2 - offsetX, _OffsetH, centerpart, booleanCP, null);
                            AddContourPoint(0 - hkat, -(_Height + _Height1 + vkat), centerpart, booleanCP, null);
                            AddContourPoint(0, -(_Height + _Height1 + _Radius), centerpart, booleanCP, new Chamfer(_Radius, 0, Chamfer.ChamferTypeEnum.CHAMFER_ARC_POINT));
                            AddContourPoint(hkat, -(_Height + _Height1 + vkat), centerpart, booleanCP, null);
                            AddContourPoint(_Width + _Width1 / 2 + offsetX, _OffsetH, centerpart, booleanCP, null);
                        }
                        break;
                    case 3:
                        {
                            //Подготовка данных для получения точки касательной к окружности
                            double hyp = Math.Sqrt(Math.Pow(_Width + _Width1 - _Width3, 2) + Math.Pow(_Height + _Height1, 2));
                            double angle1 = Math.Acos(_Radius / hyp);
                            double angle2 = Math.Acos((_Height + _Height1) / hyp);
                            double angle3 = (angle1 + angle2) - 90 * Math.PI / 180;
                            double hkat = _Radius * Math.Cos(angle3);
                            double vkat = _Radius * Math.Sin(angle3);
                            //Подготовка данных для получения точки на окружности при смещении от продольного ребра
                            double vkat1 = Math.Sqrt(Math.Pow(_Radius, 2) - Math.Pow((_Width2 + _Width3), 2));
                            //Координата Х для удлинения выреза
                            double offsetX = _OffsetH * Math.Tan(angle3);
                            switch (_TypeChamfer)
                            {
                                case 0:
                                    AddContourPoint(_Width1 / 2 + _Width2 + _DimensionF1 + _OffsetH, _OffsetH, centerpart, booleanCP, null);
                                    AddContourPoint(_Width1 / 2 + _Width2, 0 - _DimensionF1, centerpart, booleanCP, null);
                                    AddContourPoint(_Width1 / 2 + _Width2, 0 - (_Height + _Height1 - vkat1), centerpart, booleanCP, null);
                                    AddContourPoint(_Width1 / 2 - _Width3, 0 - (_Height + _Height1 + _Radius), centerpart, booleanCP, new Chamfer(_Radius, 0, Chamfer.ChamferTypeEnum.CHAMFER_ARC_POINT));
                                    AddContourPoint(_Width1 / 2 - _Width3 - hkat, 0 - (_Height + _Height1 + vkat), centerpart, booleanCP, null);
                                    AddContourPoint(0 - _Width - _Width1 / 2 - offsetX, _OffsetH, centerpart, booleanCP, null);
                                    break;
                                case 1:
                                    AddContourPoint(_Width1 / 2 + _Width2 + _DimensionF1, _OffsetH, centerpart, booleanCP, null);
                                    AddContourPoint(_Width1 / 2 + _Width2 + _DimensionF1, 0 - _DimensionF1, centerpart, booleanCP, new Chamfer(_DimensionF1, 0, Chamfer.ChamferTypeEnum.CHAMFER_ROUNDING));
                                    AddContourPoint(_Width1 / 2 + _Width2, 0 - _DimensionF1, centerpart, booleanCP, null);
                                    AddContourPoint(_Width1 / 2 + _Width2, 0 - (_Height + _Height1 - vkat1), centerpart, booleanCP, null);
                                    AddContourPoint(_Width1 / 2 - _Width3, 0 - (_Height + _Height1 + _Radius), centerpart, booleanCP, new Chamfer(_Radius, 0, Chamfer.ChamferTypeEnum.CHAMFER_ARC_POINT));
                                    AddContourPoint(_Width1 / 2 - _Width3 - hkat, 0 - (_Height + _Height1 + vkat), centerpart, booleanCP, null);
                                    AddContourPoint(0 - _Width - _Width1 / 2 - offsetX, _OffsetH, centerpart, booleanCP, null);
                                    break;
                                case 2:
                                    AddContourPoint(_Width1 / 2 + _Width2 + _DimensionF2, _OffsetH, centerpart, booleanCP, null);
                                    AddContourPoint(_Width1 / 2 + _Width2 + _DimensionF2, 0 - _DimensionF3, centerpart, booleanCP, new Chamfer(_DimensionF1, 0, Chamfer.ChamferTypeEnum.CHAMFER_ROUNDING));
                                    AddContourPoint(_Width1 / 2 + _Width2, 0 - _DimensionF3, centerpart, booleanCP, null);
                                    AddContourPoint(_Width1 / 2 + _Width2, 0 - (_Height + _Height1 - vkat1), centerpart, booleanCP, null);
                                    AddContourPoint(_Width1 / 2 - _Width3, 0 - (_Height + _Height1 + _Radius), centerpart, booleanCP, new Chamfer(_Radius, 0, Chamfer.ChamferTypeEnum.CHAMFER_ARC_POINT));
                                    AddContourPoint(_Width1 / 2 - _Width3 - hkat, 0 - (_Height + _Height1 + vkat), centerpart, booleanCP, null);
                                    AddContourPoint(0 - _Width - _Width1 / 2 - offsetX, _OffsetH, centerpart, booleanCP, null);
                                    break;
                            }
                        }
                        break;
                    case 4:
                        {
                            //Подготовка данных для получения точки касательной к окружности
                            double hyp = Math.Sqrt(Math.Pow(_Width + _Width1 - _Width4, 2) + Math.Pow(_Height + _Height1, 2));
                            double angle1 = Math.Acos(_Radius / hyp);
                            double angle2 = Math.Acos((_Height + _Height1) / hyp);
                            double angle3 = (angle1 + angle2) - 90 * Math.PI / 180;
                            double hkat = _Radius * Math.Cos(angle3);
                            double vkat = _Radius * Math.Sin(angle3);
                            //Подготовка данных для получения точки на окружности при смещении от продольного ребра
                            double vkat1 = Math.Sqrt(Math.Pow(_Radius, 2) - Math.Pow((_Width2 + _Width3), 2));
                            //Координата Х для удлинения выреза
                            double offsetX = _OffsetH * Math.Tan(angle3);

                            double hkat1 = (_Radius - vkat) * Math.Tan(angle3);

                            switch (_TypeChamfer)
                            {
                                case 0:
                                    AddContourPoint(_Width1 / 2 + _Width2 + _DimensionF1 + _OffsetH, _OffsetH, centerpart, booleanCP, null);
                                    AddContourPoint(_Width1 / 2 + _Width2, 0 - _DimensionF1, centerpart, booleanCP, null);
                                    AddContourPoint(_Width1 / 2 + _Width2, 0 - (_Height + _Height1 - _Radius), centerpart, booleanCP, null);
                                    AddContourPoint(_Width1 / 2 + _Width2 + _Width3 + _Radius, 0 - (_Height + _Height1 - _Radius), centerpart, booleanCP, new Chamfer(_Radius, 0, Chamfer.ChamferTypeEnum.CHAMFER_ROUNDING));
                                    AddContourPoint(_Width1 / 2 + _Width2 + _Width3 + _Radius, 0 - (_Height + _Height1 + _Radius), centerpart, booleanCP, new Chamfer(_Radius, 0, Chamfer.ChamferTypeEnum.CHAMFER_ROUNDING));

                                    AddContourPoint(_Width1 / 2 + _Width3, 0 - (_Height + _Height1 + _Radius), centerpart, booleanCP, null);

                                    AddContourPoint(_Width1 / 2 - _Width4 - hkat + hkat1, 0 - (_Height + _Height1 + _Radius), centerpart, booleanCP, new Chamfer(_Radius, 0, Chamfer.ChamferTypeEnum.CHAMFER_ROUNDING));
                                    AddContourPoint(0 - _Width - _Width1 / 2 - offsetX, _OffsetH, centerpart, booleanCP, null);
                                    break;
                                case 1:
                                    AddContourPoint(_Width1 / 2 + _Width2 + _DimensionF1, _OffsetH, centerpart, booleanCP, null);
                                    AddContourPoint(_Width1 / 2 + _Width2 + _DimensionF1, 0 - _DimensionF1, centerpart, booleanCP, new Chamfer(_DimensionF1, 0, Chamfer.ChamferTypeEnum.CHAMFER_ROUNDING));
                                    AddContourPoint(_Width1 / 2 + _Width2, 0 - _DimensionF1, centerpart, booleanCP, null);
                                    AddContourPoint(_Width1 / 2 + _Width2, 0 - (_Height + _Height1 - _Radius), centerpart, booleanCP, null);
                                    AddContourPoint(_Width1 / 2 + _Width2 + _Width3 + _Radius, 0 - (_Height + _Height1 - _Radius), centerpart, booleanCP, new Chamfer(_Radius, 0, Chamfer.ChamferTypeEnum.CHAMFER_ROUNDING));
                                    AddContourPoint(_Width1 / 2 + _Width2 + _Width3 + _Radius, 0 - (_Height + _Height1 + _Radius), centerpart, booleanCP, new Chamfer(_Radius, 0, Chamfer.ChamferTypeEnum.CHAMFER_ROUNDING));

                                    AddContourPoint(_Width1 / 2 + _Width3, 0 - (_Height + _Height1 + _Radius), centerpart, booleanCP, null);

                                    AddContourPoint(_Width1 / 2 - _Width4 - hkat + hkat1, 0 - (_Height + _Height1 + _Radius), centerpart, booleanCP, new Chamfer(_Radius, 0, Chamfer.ChamferTypeEnum.CHAMFER_ROUNDING));
                                    AddContourPoint(0 - _Width - _Width1 / 2 - offsetX, _OffsetH, centerpart, booleanCP, null);
                                    break;
                                case 2:
                                    AddContourPoint(_Width1 / 2 + _Width2 + _DimensionF2, _OffsetH, centerpart, booleanCP, null);
                                    AddContourPoint(_Width1 / 2 + _Width2 + _DimensionF2, 0 - _DimensionF3, centerpart, booleanCP, new Chamfer(_DimensionF1, 0, Chamfer.ChamferTypeEnum.CHAMFER_ROUNDING));
                                    AddContourPoint(_Width1 / 2 + _Width2, 0 - _DimensionF3, centerpart, booleanCP, null);
                                    AddContourPoint(_Width1 / 2 + _Width2, 0 - (_Height + _Height1 - _Radius), centerpart, booleanCP, null);
                                    AddContourPoint(_Width1 / 2 + _Width2 + _Width3 + _Radius, 0 - (_Height + _Height1 - _Radius), centerpart, booleanCP, new Chamfer(_Radius, 0, Chamfer.ChamferTypeEnum.CHAMFER_ROUNDING));
                                    AddContourPoint(_Width1 / 2 + _Width2 + _Width3 + _Radius, 0 - (_Height + _Height1 + _Radius), centerpart, booleanCP, new Chamfer(_Radius, 0, Chamfer.ChamferTypeEnum.CHAMFER_ROUNDING));

                                    AddContourPoint(_Width1 / 2 + _Width3, 0 - (_Height + _Height1 + _Radius), centerpart, booleanCP, null);

                                    AddContourPoint(_Width1 / 2 - _Width4 - hkat + hkat1, 0 - (_Height + _Height1 + _Radius), centerpart, booleanCP, new Chamfer(_Radius, 0, Chamfer.ChamferTypeEnum.CHAMFER_ROUNDING));
                                    AddContourPoint(0 - _Width - _Width1 / 2 - offsetX, _OffsetH, centerpart, booleanCP, null);
                                    break;
                            }
                        }
                        break;
                    case 5:
                        {
                            //Подготовка данных для получения точки касательной к окружности
                            double hyp = Math.Sqrt(Math.Pow(_Width + _Width1 - _Width4, 2) + Math.Pow(_Height + _Height1, 2));
                            double angle1 = Math.Acos(_Radius / hyp);
                            double angle2 = Math.Acos((_Height + _Height1) / hyp);
                            double angle3 = (angle1 + angle2) - 90 * Math.PI / 180;
                            double hkat = _Radius * Math.Cos(angle3);
                            double vkat = _Radius * Math.Sin(angle3);
                            //Подготовка данных для получения точки на окружности при смещении от продольного ребра
                            double vkat1 = Math.Sqrt(Math.Pow(_Radius, 2) - Math.Pow((_Width1 / 2 + _Width2), 2));

                            switch (_TypeChamfer)
                            {
                                case 0:
                                    AddContourPoint(-_Width1 / 2 - _Width2 - _DimensionF1 - _OffsetH, _OffsetH, centerpart, booleanCP, null);
                                    AddContourPoint(-_Width1 / 2 - _Width2, -_DimensionF1, centerpart, booleanCP, null);
                                    AddContourPoint(-_Width1 / 2 - _Width2, -(_Height + _Height1 - vkat1), centerpart, booleanCP, null);
                                    AddContourPoint(0, -(_Height + _Height1 + _Radius), centerpart, booleanCP, new Chamfer(_Radius, 0, Chamfer.ChamferTypeEnum.CHAMFER_ARC_POINT));
                                    AddContourPoint(_Width1 / 2 + _Width2, -(_Height + _Height1 - vkat1), centerpart, booleanCP, null);
                                    AddContourPoint(_Width1 / 2 + _Width2, -_DimensionF1, centerpart, booleanCP, null);
                                    AddContourPoint(_Width1 / 2 + _Width2 + _DimensionF1 + _OffsetH, _OffsetH, centerpart, booleanCP, null);
                                    break;
                                case 1:
                                    AddContourPoint(-_Width1 / 2 - _Width2 - _DimensionF1, _OffsetH, centerpart, booleanCP, null);
                                    AddContourPoint(-_Width1 / 2 - _Width2 - _DimensionF1, -_DimensionF1, centerpart, booleanCP, new Chamfer(_DimensionF1, 0, Chamfer.ChamferTypeEnum.CHAMFER_ROUNDING));
                                    AddContourPoint(-_Width1 / 2 - _Width2, -_DimensionF1, centerpart, booleanCP, null);
                                    AddContourPoint(-_Width1 / 2 - _Width2, -(_Height + _Height1 - vkat1), centerpart, booleanCP, null);
                                    AddContourPoint(0, -(_Height + _Height1 + _Radius), centerpart, booleanCP, new Chamfer(_Radius, 0, Chamfer.ChamferTypeEnum.CHAMFER_ARC_POINT));
                                    AddContourPoint(_Width1 / 2 + _Width2, -(_Height + _Height1 - vkat1), centerpart, booleanCP, null);
                                    AddContourPoint(_Width1 / 2 + _Width2, -_DimensionF1, centerpart, booleanCP, null);
                                    AddContourPoint(_Width1 / 2 + _Width2 + _DimensionF1, -_DimensionF1, centerpart, booleanCP, new Chamfer(_DimensionF1, 0, Chamfer.ChamferTypeEnum.CHAMFER_ROUNDING));
                                    AddContourPoint(_Width1 / 2 + _Width2 + _DimensionF1, _OffsetH, centerpart, booleanCP, null);
                                    break;
                                case 2:
                                    AddContourPoint(-_Width1 / 2 - _Width2 - _DimensionF2, _OffsetH, centerpart, booleanCP, null);
                                    AddContourPoint(-_Width1 / 2 - _Width2 - _DimensionF2, -_DimensionF3, centerpart, booleanCP, new Chamfer(_DimensionF1, 0, Chamfer.ChamferTypeEnum.CHAMFER_ROUNDING));
                                    AddContourPoint(-_Width1 / 2 - _Width2, -_DimensionF3, centerpart, booleanCP, null);
                                    AddContourPoint(-_Width1 / 2 - _Width2, -(_Height + _Height1 - vkat1), centerpart, booleanCP, null);
                                    AddContourPoint(0, -(_Height + _Height1 + _Radius), centerpart, booleanCP, new Chamfer(_Radius, 0, Chamfer.ChamferTypeEnum.CHAMFER_ARC_POINT));
                                    AddContourPoint(_Width1 / 2 + _Width2, -(_Height + _Height1 - vkat1), centerpart, booleanCP, null);
                                    AddContourPoint(_Width1 / 2 + _Width2, -_DimensionF3, centerpart, booleanCP, null);
                                    AddContourPoint(_Width1 / 2 + _Width2 + _DimensionF2, -_DimensionF3, centerpart, booleanCP, new Chamfer(_DimensionF1, 0, Chamfer.ChamferTypeEnum.CHAMFER_ROUNDING));
                                    AddContourPoint(_Width1 / 2 + _Width2 + _DimensionF2, _OffsetH, centerpart, booleanCP, null);
                                    break;
                            }
                        }
                        break;
                }
                //Для перемещения выреза по центру детали
                selectedpoint1.Z = centerpart;
                CoordinateSystem startCS = new CoordinateSystem(
                    new TSG.Point(0, 0, centerpart),
                    new TSG.Vector(1, 0, 0),
                    new TSG.Vector(0, 0, 1000));
                CoordinateSystem endCS = new CoordinateSystem(
                    selectedpoint1,
                    new TSG.Vector(selectedpoint2.X - selectedpoint1.X, selectedpoint2.Y - selectedpoint1.Y, 0),
                    new TSG.Vector(0, 0, 1000));
                //Тип отзеркаливания выреза 
                switch (_Mirror)
                {
                    case 0:
                        break;
                    case 1:
                        endCS.AxisX = new TSG.Vector(-(selectedpoint2.X - selectedpoint1.X), -(selectedpoint2.Y - selectedpoint1.Y), 0); //что то не так. разобраться
                        endCS.AxisY = new TSG.Vector(0, 0, -1000);
                        break;
                    case 2:
                        endCS.AxisY = new TSG.Vector(0, 0, -1000);
                        break;
                }

                booleanCP.Class = BooleanPart.BooleanOperativeClassName;
                booleanCP.Insert();
                Operation.MoveObject(booleanCP, startCS, endCS);

                BooleanPart booleanPart = new BooleanPart();
                booleanPart.Father = selectedPart;
                booleanPart.SetOperativePart(booleanCP);
                booleanPart.Insert();
                booleanCP.Delete();
                Operation.DisplayPrompt("Готово");
                */
            }
            catch (Exception Exc)
            {
                MessageBox.Show("Ошибка в start");
                MessageBox.Show(Exc.ToString());
            }
            
            return true;

            //Добавление контурных точек в контурную пластину
            void AddContourPoint(double x, double y, double z, ContourPlate cp, Chamfer chamfer)
            {
                ContourPoint point = new ContourPoint(new TSG.Point(x, y, z), chamfer);
                cp.AddContourPoint(point);
            }
        }
            

        #region Private methods
        /// <summary>
        /// Gets the values from the dialog and sets the default values if needed
        /// </summary>
        private void GetValuesFromDialog()
        {
            _Height = Data.height;
            _Height1 = Data.height1;
            _Width = Data.width;
            _Width1 = Data.width1;
            _Width2 = Data.width2;
            _Width3 = Data.width3;
            _Width4 = Data.width4;
            _Radius = Data.radius;
            _OffsetH = Data.offsetH;
            _OffsetL = Data.offsetL;
            _DimensionF1 = Data.dimensionF1;
            _DimensionF2 = Data.dimensionF2;
            _DimensionF3 = Data.dimensionF3;
            _TypeCut = Data.typeCut;
            _Mirror = Data.mirror;
            _TypeChamfer = Data.typeChamfer;




            if (IsDefaultValue(_Height))
                _Height = 50;
            if (IsDefaultValue(_Height1))
                _Height1 = 0;
            if (IsDefaultValue(_Width))
                _Width = 50;
            if (IsDefaultValue(_Width1))
                _Width1 = 30;
            if (IsDefaultValue(_Width2))
                _Width2 = 50;
            if (IsDefaultValue(_Width3))
                _Width3 = 50;
            if (IsDefaultValue(_Width4))
                _Width4 = 50;
            if (IsDefaultValue(_Radius))
                _Radius = 12.5;
            if (IsDefaultValue(_OffsetH))
                _OffsetH = 0;
            if (IsDefaultValue(_OffsetL))
                _OffsetL = 0;
            if (IsDefaultValue(_DimensionF1))
                _DimensionF1 = 20;
            if (IsDefaultValue(_DimensionF2))
                _DimensionF2 = 50;
            if (IsDefaultValue(_DimensionF3))
                _DimensionF3 = 50;
            if (IsDefaultValue(_TypeCut))
                _TypeCut = 0;
            if (IsDefaultValue(_Mirror))
                _Mirror = 0;
            if (IsDefaultValue(_TypeChamfer))
                _TypeChamfer = 0;
        }

        // Write your private methods here.

        #endregion
    }
}
