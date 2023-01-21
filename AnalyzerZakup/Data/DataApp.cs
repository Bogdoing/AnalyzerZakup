using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalyzerZakup
{
    static class DataApp
    {
        public static string TxtBoxfilepath { get; set; }

        public static string TxtBoxFileDB =
        "Data Source=DESKTOP-432U1GM\\SQLEXPRESS;Initial Catalog=AnalizeXml2;Integrated Security=True;MultipleActiveResultSets=True;"; //AnalizeXML

        public static bool checkBox1 { get; set; }
        public static bool checkBox2 { get; set; }
        public static bool checkBox3 { get; set; }
        public static bool checkBox4 { get; set; }
        public static bool checkBox5 { get; set; }
        public static bool checkBox6 { get; set; }

        public static string region { get; set; }

        public static string[] mas_resion = {
                "Все регионы",
                "Алтайский край / Altajskij_kraj",
                "Амурская область / Amurskaja_ob1",
                "Архангельская область / Arkhangelskaja_obl",
                "Астраханская область / Astrakhanskaja_obl",

                "Байконур / Bajkonur_g",
                "Башкиртостанская республика / Bashkortostan_Resp",
                "Белгородская область / Belgorodskaja_obl",
                "Бржанская область / Brjanskaja_obl",
                "Бурятская республика / Burjatija Resp",

                "Владимирская область / Vladimirskaja_obl",
                "Волгоградская область / Volgogradskaja_obl",
                "Вологодская область / Vologodskaja_obl",
                "Воронежская область / Voronezhskaja_obl",

                "Дагестанская беспублика / Dagestan_Resp",

                "Еврейская автономная область / Evrejskaja_Aobl",

                "Забайкальский край / Zabajkalskij_kraj",

                "Ингушетия / Ingushetija_Resp",
                "Иркутская область / Irkutskaja_obl",
                "Ивановская область / Ivanovskaja_obl",

                "Кировская область / Kirovskaja_obl",
                "Костромская область / Kostromskaja_obl",
                "Краснодарский край / Krasnodarskij_kraj",
                "Красноярский край / Krasnojarskij_kraj",
                "Кабардино-Балкария / Kabardino-Balkarskaja_Resp",
                "Калининградская область / Kaliningradskaja_obl",
                "Калмыкия / Kalmykija_Resp",
                "Калужская область / Kaluzhskaja_obl",
                "Камчатский край / Kamchatskij_kraj",
                "Карачаево-Черкесия / Karachaevo-Cherkesskaja_Resp",
                "Карелия / Karelija_Resp",
                "Кемеровская область / Kemerovskaja_obl",
                "Курганская область / Kurganskaja_obl",
                "Курская область / Kurskaja_obl",

                "Ленинградская область / Leningradskaja_obl",
                "Липецкая область / Lipeckaja_obl",

                "Магаданская область / Magadanskaja_obl",
                "Марий Эл / Marij_E1_Resp",
                "Мордовия / Mordovija_Resp",
                "Московская область / Moskovskaja_obl",
                "Москва / Moskva",
                "Мурманская область / Murmanskaja_obl",

                "Ненецкий автономный округ / Neneckij_AO",
                "Нижегородская область / Nizhegorodskaja_obl",
                "Новгородская область / Novgorodskaja_obl",
                "Новосибирская область / Novosibirskaja_obl",

                "Омская область / Omskaja_obl",
                "Оренбургская область / Orenburgskaja_obl",
                "Орловская область / Orlovskaja_obl",

                "Пензенская область / Penzenskaja_obl",
                "Пермский край / Permskij_kraj",
                "Приморский край / Primorskij_kraj",
                "Псковская область / Pskovskaja_obl",

                "Республика Крым / Krim_Resp",
                "Республика Коми / Komi_Resp",
                "Республика Чувашия Chuvashskaja_Resp",
                "Рязанская область / Rjazanskaja_obl",
                "Ростовская область / Rostovskaja_obl",

                "Сахалинская область / Sakhalinskaja_obl",
                "Самарская область / Samarskaja_obl",
                "Санкт-Петербург / Sankt-Peterburg",
                "Саратовская область / Saratovskaja_obl",
                "Севастополь / Sevastopol_g",
                "Северная Осетия / Severnaja_Osetija-Alanija_Resp",
                "Смоленская область / Smolenskaja_obl",
                "Ставропольский край / Stavropolskij_kraj",
                "Свердловская область / Sverdlovskaja_obl",

                "Тамбовская область / Tambovskaja_obl",
                "Татарстан / Tatarstan_Resp",
                "Тюменская область / Tjumenskaja_obl",
                "Томская область / Tomskaja_obl",
                "Тульская область / Tulskaja_obl",
                "Тверская область / Tverskaja_obl",
                "Тыва / Tyva_Resp",

                "Удмуртия / Udmurtskaja_Resp",
                "Ульяновская область / Uljanovskaja_obl",

                "Хабаровский край / Khabarovskij_kraj",
                "Хакасия / Khakasija_Resp",
                "Ханты-Мансийский АО - Югра / Khanty-Mansijskij_AO-Jugra_AO",

                "Чеченская республика / Chechenskaja_Resp",
                "Челябинская область / Cheljabinskaja_obl",
                "Чукотский АО / Chukotskij_AO",

                "Ямало-Ненецкий АО / Jamalo-Neneckij_AO",
                "Ярославская область / Jaroslavskaja_obl",
                "Якутия / Sakha_Jakutija_Resp",

                };
    }
}
