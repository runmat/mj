using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using GeneralTools.Models;
using GeneralTools.Services;
using WpfTools4.ViewModels;

namespace WpfComboboxAutocomplete
{
    public class MainViewModel : ViewModelBase, IAutoCompleteTagCloudConsumer
    {
        string _selectedTag;
        ObservableCollection<Tag> _tags;
        ObservableCollection<Tag> _selectedTags;
        const string TagsFileName = @"C:\Backup\Tags.xml";

        public string SelectedTag
        {
            get { return _selectedTag; }
            set { _selectedTag = value; SendPropertyChanged("SelectedTag"); }
        }


        public ObservableCollection<Tag> Tags
        {
            get
            {
                if (_tags != null)
                    return _tags;

                //var text = @"Es steht völlig außer Frage dass der Zuzug so vieler Menschen uns noch einiges abverlangen wird sagt Merkel laut vorab verbreitetem Text in ihrer Neujahrsansprache Das wird Zeit Kraft und Geld kosten  gerade mit Blick auf die so wichtige Aufgabe der Integration derer die dauerhaft hier bleiben werden sagt die Kanzlerin Sie sprach von einer besonders herausfordernden Zeit Zugleich aber gelte: Wir schaffen das denn Deutschland ist ein starkes Land Merkel zeigte sich überzeugt dass die heutige große Aufgabe des Zuzugs und der Integration so vieler Menschen eine Chance von morgen sei wenn die Sache richtig angepackt werde Von gelungener Einwanderung habe ein Land noch immer profitiert  wirtschaftlich wie gesellschaftlich so Merkel Dabei sei das großartige bürgerschaftliche Engagement ebenso hilfreich wie ein umfassendes Konzept politischer Maßnahmen Deutschland habe in der Vergangenheit schon viele große Herausforderungen gemeistert und sei noch immer an ihnen gewachsen So sei das Land in den Jahren seit der Wiedervereinigung zusammengewachsen stehe wirtschaftlich stark da und habe die niedrigste Arbeitslosigkeit und die höchste Erwerbstätigkeit des geeinten Deutschlands Die Kanzlerin will die Zahl der Flüchtlinge spürbar verringern Merkel würdigte wie zuvor schon Bundespräsident Joachim Gauck in seiner Weihnachtsansprache die große Hilfsbereitschaft der Deutschen Ich danke den unzähligen freiwilligen Helfern für ihre Herzenswärme und ihre Einsatzbereitschaft die immer mit diesem Jahr verbunden sein werden so Merkel Sie danke aber auch den hauptamtlichen Helfern Polizisten Soldaten sowie Mitarbeitern der Behörden im Bund in den Ländern und in den Kommunen Sie alle tun weit weit mehr als das was ihre Pflicht ist sagte die Kanzlerin Die Bundesregierung arbeite daran die Zahl der Flüchtlinge nachhaltig und dauerhaft spürbar zu verringern Mit Blick auf die Integration der Flüchtlinge in Deutschland sprach Merkel davon aus Fehlern der Vergangenheit zu lernen Unsere Werte unsere Traditionen unser Rechtsverständnis unsere Sprache unsere Gesetze unsere Regeln  sie tragen unsere Gesellschaft und sie sind Grundvoraussetzung für ein gutes ein von gegenseitigem Respekt geprägtes Zusammenleben aller in unserem Land so die Kanzlerin Das gelte für jeden der hier leben will Die Neujahrsansprache der Kanzlerin wird am Silvesterabend von mehreren Fernsehsendern ausgestrahlt";

                _tags = new ObservableCollection<Tag>(
                    TagsLoad()
                    //text.Split(' ')
                    //.Where(t => t.Trim().Length > 0)
                    //.GroupBy(g => g)
                    //.Select(t => new Tag { Name = t.Key.Trim(' ', '\t', '\r', '\n'), Parent = this})
                );

                var source = CollectionViewSource.GetDefaultView(_tags);
                source.SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));

                return _tags;
            }
        }
        public ObservableCollection<Tag> SelectedTags
        {
            get
            {
                if (_selectedTags != null)
                    return _selectedTags;

                _selectedTags = new ObservableCollection<Tag>();
                var source = CollectionViewSource.GetDefaultView(_selectedTags);
                source.SortDescriptions.Add(new SortDescription("Sort", ListSortDirection.Ascending));

                return _selectedTags;
            }
        }

        public void OnDropDownTabKey(string tagText)
        {
            if (tagText.IsNullOrEmpty())
                return;

            if (Tags.None(t => t.Name.ToLower() == tagText.ToLower()))
            {
                Tags.Add(new Tag { Name = tagText.ToLowerFirstUpper(), Parent = this });
                TagsSave();
            }

            if (SelectedTags.None(t => t.Name.ToLower() == tagText.ToLower()))
                SelectedTags.Add(new Tag { Name = tagText.ToLowerFirstUpper(), Sort = SelectedTags.Count, Parent = this });
        }

        public void OnDeleteTag(string tagText)
        {
            var selectedTag = SelectedTags.FirstOrDefault(t => t.Name == tagText);
            if (selectedTag != null)
                SelectedTags.Remove(selectedTag);

            var tag = Tags.FirstOrDefault(t => t.Name == tagText);
            if (tag != null)
            { 
                Tags.Remove(tag);
                TagsSave();
            }
        }

        void TagsSave()
        {
            XmlService.XmlSerializeToFile(Tags.ToList(), TagsFileName);
        }

        List<Tag> TagsLoad()
        {
            var list = XmlService.XmlDeserializeFromFile<List<Tag>>(TagsFileName);
            list.ForEach(t => t.Parent = this);

            return list;
        }
    }
}
