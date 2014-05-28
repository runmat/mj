using System;
using GeneralTools.Models;
using NUnit.Framework;

namespace GeneralTools.UnitTests.ModelMappings
{
    [TestFixture]
    public class Tests
    {
        /// <summary>
        /// Test auf Funktionsweise des Kopierens einer Model Klasse auf eine andere Model Klasse, wobei Properties mit gleichen Namen automatisch kopiert werden
        /// </summary>
        [Test]
        public void ModelMappingCopy()
        {
            var c1 = new TestClass1Initialized();
            var c2 = new TestClass2();

            // ... ModelMapping.Copy() kopiert Properties mit gleichen Namen automatisch zwischen zwei Model Klassen
            ModelMapping.Copy(c1, c2);

            Assert.AreEqual(c1.BoolProperty, c2.BoolProperty, "ModelMapping.Copy, eine bool Property wurde nicht kopiert");
            Assert.AreEqual(c1.NullableBoolProperty, c2.NullableBoolProperty, "ModelMapping.Copy, eine bool? Property wurde nicht kopiert");
            
            Assert.AreEqual(c1.IntProperty, c2.IntProperty, "ModelMapping.Copy, eine int Property wurde nicht kopiert");
            Assert.AreEqual(c1.NullableIntProperty, c2.NullableIntProperty, "ModelMapping.Copy, eine int? Property wurde nicht kopiert");
            
            Assert.AreEqual(c1.LongProperty, c2.LongProperty, "ModelMapping.Copy, eine long Property wurde nicht kopiert");
            Assert.AreEqual(c1.NullableLongProperty, c2.NullableLongProperty, "ModelMapping.Copy, eine long? Property wurde nicht kopiert");

            Assert.AreEqual(c1.DateTimeProperty, c2.DateTimeProperty, "ModelMapping.Copy, eine DateTime Property wurde nicht kopiert");
            Assert.AreEqual(c1.NullableDateTimeProperty, c2.NullableDateTimeProperty, "ModelMapping.Copy, eine DateTime? Property wurde nicht kopiert");

            Assert.AreEqual(c1.StringProperty1, c2.StringProperty1, "ModelMapping.Copy, eine string Property wurde nicht kopiert");
        }

        /// <summary>
        /// Test auf Funktionsweise von Property Attribut "ModelMappingCopyIgnore"
        /// </summary>
        [Test]
        public void ModelMappingCopyIgnore()
        {
            var c1 = new TestClass1Initialized();
            var c2 = new TestClass2();

            // ... ModelMapping.Copy() darf keine Properties mit Attribut "ModelMappingCopyIgnore" kopieren!
            ModelMapping.Copy(c1, c2);

            Assert.IsFalse(c1.StringPropertyModelMappingCopyIgnore.NotNullOrEmpty() == c2.StringPropertyModelMappingCopyIgnore.NotNullOrEmpty());
        }

        /// <summary>
        /// Test auf Funktionsweise von Property Attribut "ModelMappingCompareIgnore"
        /// </summary>
        [Test]
        public void ModelMappingCompareIgnore()
        {
            var c1 = new TestClass1Initialized();
            var c2 = ModelMapping.CloneBySerializing(c1);

            // Diese Property ist versehen mit Attribut "ModelMappingCompareIgnore"
            // c1.Property weicht nun ab von c2.Property ...
            c2.StringPropertyModelMappingCompareIgnore = "changed";

            // ... ModelMapping.Differences() darf keine Auflistung zurückgeben, ergo es darf keine "Differences" geben!
            Assert.IsTrue(ModelMapping.Differences(c1, c2).None());
        }

        /// <summary>
        /// Test auf Funktionsweise von Property Attribut "ModelMappingClearable"
        /// </summary>
        [Test]
        public void ModelMappingClearable()
        {
            var c1 = new TestClass1Initialized();

            // ... ModelMapping.Clear() darf nur Properties mit Attribut "ModelMappingClearable" löschen!
            ModelMapping.Clear(c1);

            // nur ok, wenn diese Property gelöscht wurde (leer ist)
            Assert.IsTrue(c1.StringPropertyModelMappingClearable.IsNullOrEmpty(), "'StringPropertyModelMappingClearable' wurde NICHT gelöscht obwohl sie das Attribut 'ModelMappingClearable' besitzt");

            // nur ok, wenn diese Property nicht gelöscht wurde (nicht leer ist)
            Assert.IsFalse(c1.StringProperty1.IsNullOrEmpty(), "'StringProperty1' wurde gelöscht obwohl sie das Attribut 'ModelMappingClearable' nicht besitzt");
        }

        /// <summary>
        /// Test auf Funktionsweise Kopieren von Boolean Properties zu einem String (konvertiert nach z. B. "X") 
        /// </summary>
        [Test]
        public void ModelMappingCopyBoolToString()
        {
            var c1 = new TestClass1Initialized();
            var c2 = new TestClass2();

            ModelMapping.Copy(c1, c2);

            Assert.IsTrue(c2.BoolPropertyToConvert.NotNullOrEmpty() == "X", "'BoolPropertyToConvert' wurde NICHT korrekt zum Wert 'X' konvertiert");

            c1.BoolPropertyToConvert = false;
            ModelMapping.Copy(c2, c1);
            Assert.IsTrue(c1.BoolPropertyToConvert, "'BoolProperty' wurde NICHT korrekt zum Wert 'true' zurück konvertiert");
        }

        /// <summary>
        /// Test auf erweitertes ModelMapping zwischen Properties mit unterschiedlichen Namen
        /// über statische Klasse z. B. namens "AppModelMappings" die von Klasse "ModelMappings" abgeleitet ist
        /// </summary>
        [Test]
        public void AppModelMappingsTest()
        {
            var c1 = new AdvancedTestClass1
                {
                    BoolProperty = true,
                    DateTimeProperty = DateTime.Today,
                    IntProperty = 42,
                    StringProperty = "test",
                };
            var c2 = new AdvancedTestClass2();

            // 1. Copy Test
            AppModelMappings.MapAdvancedClasses.Copy(c1, c2,
                    // UserCode Init Copy:
                    (source, destination) =>
                        {
                            destination.StringPropertyCopyChangedByUserCode = "changed by Copy UserCode";
                        }
            );

            Assert.IsTrue(c1.BoolProperty == c2.BoolPropertyDifferentName, "AppModelMappings Copy, kopierte nicht die BoolProperty");
            Assert.IsTrue(c1.IntProperty == c2.IntPropertyDifferentName, "AppModelMappings Copy, kopierte nicht die IntProperty");
            Assert.IsTrue(c1.DateTimeProperty == c2.DateTimePropertyDifferentName, "AppModelMappings Copy, kopierte nicht die DateTimeProperty");
            Assert.IsTrue(c1.StringProperty == c2.StringPropertyDifferentName, "AppModelMappings Copy, kopierte nicht die StringProperty");

            Assert.AreEqual("changed by Copy AppCode", c2.StringPropertyCopyChangedByAppCode.NotNullOrEmpty(), "AppModelMappings Copy, AppCode.Init modifizierte eine Property nicht wie erwartet");
            Assert.AreEqual("changed by Copy UserCode", c2.StringPropertyCopyChangedByUserCode.NotNullOrEmpty(), "AppModelMappings Copy, UserCode.Init modifizierte eine Property nicht wie erwartet");

            // 2. Copy Back Test
            var c11 = new AdvancedTestClass1();
            AppModelMappings.MapAdvancedClasses.CopyBack(c2, c11,
                // UserCode Init CopyBack:
                    (source, destination) =>
                    {
                        destination.StringPropertyCopyBackChangedByUserCode = "changed by CopyBack UserCode";
                    }
            );

            Assert.IsTrue(c11.BoolProperty == c2.BoolPropertyDifferentName, "AppModelMappings CopyBack, kopierte nicht die BoolProperty");
            Assert.IsTrue(c11.IntProperty == c2.IntPropertyDifferentName, "AppModelMappings CopyBack, kopierte nicht die IntProperty");
            Assert.IsTrue(c11.DateTimeProperty == c2.DateTimePropertyDifferentName, "AppModelMappings CopyBack, kopierte nicht die DateTimeProperty");
            Assert.IsTrue(c11.StringProperty == c2.StringPropertyDifferentName, "AppModelMappings CopyBack, kopierte nicht die StringProperty");

            Assert.AreEqual("changed by CopyBack AppCode", c11.StringPropertyCopyBackChangedByAppCode.NotNullOrEmpty(), "AppModelMappings CopyBack, AppCode.Init modifizierte eine Property nicht wie erwartet");
            Assert.AreEqual("changed by CopyBack UserCode", c11.StringPropertyCopyBackChangedByUserCode.NotNullOrEmpty(), "AppModelMappings CopyBack, UserCode.Init modifizierte eine Property nicht wie erwartet");
        }
    }
}
