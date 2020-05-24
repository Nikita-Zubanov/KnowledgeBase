using KnowledgeBaseLibrary.FundamentalVariables;
using KB = KnowledgeBaseLibrary.RuleVariables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using KnowledgeBaseLibrary.RuleVariables;
using System.Globalization;
using System.Data;
using System.Data.SqlClient;
using KnowledgeBaseLibrary.InputOutputVariables;

namespace KnowledgeBaseLibrary
{
    public class DBManager : IDisposable
    {
        private SqlConnection connection;
        private const string connectionString = @"Data Source=(local);Initial Catalog=KnowledgeBaseDB;Integrated Security=True";

        public DBManager()
        {
            try
            {
                connection = new SqlConnection(connectionString);
                connection.Open();
            }
            catch { }
        }

        public void Dispose()
        {
            try
            {
                connection.Close();
            }
            catch { }
        }

        public IList<ILinguisticVariable> GetLinguisticVariables()
        {
            try
            {
                IList<ILinguisticVariable> linguisticVariables = new List<ILinguisticVariable>();

                string commandString =
                    "Select Parameters.Title, ValueRanges.ValueRange, TermSets.TermSet, Parameters.Description, Parameters.Unit " +
                    "From LinguisticVariables " +
                    "Left Join TermSets On LinguisticVariables.TermSet_ID = TermSets.id " +
                    "Left Join ValueRanges On LinguisticVariables.ValueRange_ID = ValueRanges.id " +
                    "Left Join Parameters On LinguisticVariables.Parameter_ID = Parameters.id ";
                using (SqlCommand command = new SqlCommand(commandString, connection))
                {
                    DataTable dataTable = new DataTable();
                    SqlDataReader reader = command.ExecuteReader();
                    dataTable.Load(reader);

                    linguisticVariables = GetLinguisticVariablesFromDataTable(dataTable);

                    reader.Close();
                }

                return linguisticVariables;
            }
            catch
            {
                return GetStaticLinguisticVariables();
            }
        }
        public IList<ILinguisticVariable> GetLinguisticVariables(IList<IParameter> parameters)
        {
            try
            {
                IList<ILinguisticVariable> linguisticVariables = new List<ILinguisticVariable>();
                IList<string> paramList = parameters.Select(p => $" Title = '{p.Title}' Or").ToList();
                string paramTitles = "";
                foreach (string param in paramList)
                    paramTitles += param;
                paramTitles = paramTitles.TrimEnd(new char[] { 'r', 'O' });

                string commandString =
                    "Select Parameters.Title, ValueRanges.ValueRange, TermSets.TermSet, Parameters.Description, Parameters.Unit " +
                    "From LinguisticVariables " +
                    "Left Join TermSets On LinguisticVariables.TermSet_ID = TermSets.id " +
                    "Left Join ValueRanges On LinguisticVariables.ValueRange_ID = ValueRanges.id " +
                    "Left Join Parameters On LinguisticVariables.Parameter_ID = Parameters.id " +
                    $"Where{paramTitles}";
                using (SqlCommand command = new SqlCommand(commandString, connection))
                {
                    DataTable dataTable = new DataTable();
                    SqlDataReader reader = command.ExecuteReader();
                    dataTable.Load(reader);

                    linguisticVariables = GetLinguisticVariablesFromDataTable(dataTable);

                    reader.Close();
                }

                return linguisticVariables;
            }
            catch
            {
                return GetStaticLinguisticVariables(parameters);
            }
        }

        public IList<KB.Rule> GetRules()
        {
            try
            {
                IList<KB.Rule> rules = new List<KB.Rule>();

                string commandString =
                    "Select * " +
                    "From Rules Left Join " +
                    "(Select LinguisticVariables.id, Parameters.Title, ValueRanges.ValueRange, TermSets.TermSet, Parameters.Description, Parameters.Unit " +
                    "From LinguisticVariables " +
                    "Left Join TermSets On LinguisticVariables.TermSet_ID = TermSets.id " +
                    "Left Join ValueRanges On LinguisticVariables.ValueRange_ID = ValueRanges.id " +
                    "Left Join Parameters On LinguisticVariables.Parameter_ID = Parameters.id) lv " +
                    "On Rules.LinguisticVariable_ID = lv.id ";
                using (SqlCommand command = new SqlCommand(commandString, connection))
                {
                    DataTable dataTable = new DataTable();
                    SqlDataReader reader = command.ExecuteReader();
                    dataTable.Load(reader);

                    rules = GetRulesFromDataTable(dataTable);

                    reader.Close();
                }

                return rules;
            }
            catch
            {
                return GetStaticRules();
            }
        }

        public IList<FactorFuzzyValue> GetTermSets()
        {
            try
            {
                IList<FactorFuzzyValue> termSet = new List<FactorFuzzyValue>();

                string commandString =
                    "Select TermSets.TermSet " +
                    "From TermSets ";
                using (SqlCommand command = new SqlCommand(commandString, connection))
                {
                    DataTable dataTable = new DataTable();
                    SqlDataReader reader = command.ExecuteReader();
                    dataTable.Load(reader);

                    termSet = GetTermSetsFromDataTable(dataTable);

                    reader.Close();
                }

                return termSet;
            }
            catch
            {
                return GetStaticTermSets();
            }
        }

        #region Methods for extracting linguistic variables from a data table
        private IList<ILinguisticVariable> GetLinguisticVariablesFromDataTable(DataTable dataTable)
        {
            IList<ILinguisticVariable> linguisticVariables = new List<ILinguisticVariable>();

            for (int rowIndex = 0; rowIndex < dataTable.Rows.Count; rowIndex++)
            {
                LinguisticVariable linguisticVariable = GetLinguisticVariableFromDataTableRow(dataTable.Columns, dataTable.Rows[rowIndex]);

                linguisticVariables.Add(linguisticVariable);
            }

            return linguisticVariables;
        }
        private LinguisticVariable GetLinguisticVariableFromDataTableRow(DataColumnCollection columns, DataRow row)
        {
            Dictionary<string, object> data = new Dictionary<string, object>();
            for (int col = 0; col < columns.Count; col++)
                data.Add(columns[col].ColumnName, row[col]);

            FactorTitle title = (FactorTitle)Enum.Parse(typeof(FactorTitle), data["Title"].ToString());
            IList<FactorFuzzyValue> termSet = GetTermSet(data["TermSet"].ToString());
            Dictionary<FactorFuzzyValue, decimal[]> valueRanges = GetValueRanges(data["ValueRange"].ToString(), termSet);
            string description = data["Description"].ToString();
            string unit = data["Unit"].ToString();

            return new LinguisticVariable(title: title, termSet: termSet, valueRanges: valueRanges, description: description, unit: unit);
        }
        private IList<FactorFuzzyValue> GetTermSet(string json)
        {
            json = json.Trim(' ');
            IList<FactorFuzzyValue> fuzzyValues = new List<FactorFuzzyValue>();
            IList<string> valueStrList = GetListValuesFromJson(json, "FuzzyValues");

            foreach (string valueStr in valueStrList)
            {
                FactorFuzzyValue fuzzyValue = (FactorFuzzyValue)Enum.Parse(typeof(FactorFuzzyValue), valueStr);
                fuzzyValues.Add(fuzzyValue);
            }

            return fuzzyValues;
        }
        private Dictionary<FactorFuzzyValue, decimal[]> GetValueRanges(string json, IList<FactorFuzzyValue> termSet)
        {
            if (String.IsNullOrEmpty(json))
                return null;

            Dictionary<FactorFuzzyValue, decimal[]> valueRanges = new Dictionary<FactorFuzzyValue, decimal[]>();
            Dictionary<FactorFuzzyValue, IList<string>> valueStrRanges = GetDictionaryValuesFromJson(json, termSet);

            foreach (KeyValuePair<FactorFuzzyValue, IList<string>> keyValue in valueStrRanges)
            {
                valueRanges[keyValue.Key] = new decimal[keyValue.Value.Count];

                valueRanges[keyValue.Key][0] = keyValue.Value
                    .Select(v => Decimal.Parse(v, new CultureInfo("en-GB")))
                    .First();
                valueRanges[keyValue.Key][1] = keyValue.Value
                    .Select(v => Decimal.Parse(v, new CultureInfo("en-GB")))
                    .Last();
            }

            return valueRanges;
        }
        #endregion

        #region Methods for extracting rules from a data table
        private IList<KB.Rule> GetRulesFromDataTable(DataTable dataTable)
        {
            IList<KB.Rule> rules = new List<KB.Rule>();

            for (int rowIndex = 0; rowIndex < dataTable.Rows.Count; rowIndex++)
            {
                KB.Rule rule = GetRuleFromDataTableRow(dataTable.Columns, dataTable.Rows[rowIndex]);

                rules.Add(new SimpleRule(rule.LinguisticVariable, rule.Antecedent, rule.Consequent));
            }

            return rules;
        }
        private KB.Rule GetRuleFromDataTableRow(DataColumnCollection columns, DataRow row)
        {
            Dictionary<string, object> data = new Dictionary<string, object>();
            for (int col = 0; col < columns.Count; col++)
                data.Add(columns[col].ColumnName, row[col]);

            LinguisticVariable linguisticVariable = GetLinguisticVariableFromDataTableRow(columns, row);
            Antecedent antecedent = GetAntecedentFromJson(data["Antecedent"].ToString());
            Judgment consequent = GetJudgmentFromJson(data["Consequent"].ToString());

            return new SimpleRule(linguisticVariable, antecedent, consequent);
        }
        private Antecedent GetAntecedentFromJson(string json)
        {
            Antecedent antecedent = new Antecedent();
            IList<LogicalConnection> connections = new List<LogicalConnection> { LogicalConnection.AND, LogicalConnection.OR };
            antecedent = GetAntecedentFromJsonByTermSet(json, connections);

            return antecedent;
        }
        private Antecedent GetAntecedentFromJsonByTermSet(string json, IList<LogicalConnection> termSet)
        {
            Antecedent antecedent = new Antecedent();
            string[] jsonLines = Regex.Split(json, "],");
            IEnumerator<LogicalConnection> termSetEnum = termSet.GetEnumerator();

            foreach (string jsonLine in jsonLines)
            {
                while (!jsonLine.Contains(termSetEnum.Current.ToString()))
                    termSetEnum.MoveNext();

                IList<string> result = GetListValuesFromJson(jsonLine, termSetEnum.Current.ToString());

                if (termSetEnum.Current == LogicalConnection.AND)
                    foreach (string judgmentStr in result)
                        antecedent.AND(GetJudgmentFromJson(judgmentStr));
                else if (termSetEnum.Current == LogicalConnection.OR)
                    antecedent.OR(GetJudgmentListFromString(result).ToArray());
            }

            return antecedent;
        }
        private Judgment GetJudgmentFromJson(string json)
        {
            if (String.IsNullOrEmpty(json))
                return null;

            json = json.Trim(new char[] { '\\', '"', ' ' });

            Judgment judgment = GetJudgmentListFromString(new List<string> { json }).First();

            return judgment;
        }
        private IList<Judgment> GetJudgmentListFromString(IList<string> judgmentStrList)
        {
            IList<Judgment> judgments = new List<Judgment>();
            foreach (string judgment in judgmentStrList)
            {
                string[] words = Regex.Split(judgment, "—");

                FactorTitle title = (FactorTitle)Enum.Parse(typeof(FactorTitle), words.First());
                FactorFuzzyValue fuzzyValue = (FactorFuzzyValue)Enum.Parse(typeof(FactorFuzzyValue), words.Last());

                judgments.Add(new Judgment(title, fuzzyValue));
            }

            return judgments;
        }
        #endregion

        #region Methods for extracting termsets from a data table
        private IList<FactorFuzzyValue> GetTermSetsFromDataTable(DataTable dataTable)
        {
            IList<FactorFuzzyValue> termSet = new List<FactorFuzzyValue>();

            for (int rowIndex = 0; rowIndex < dataTable.Rows.Count; rowIndex++)
            {
                IList<FactorFuzzyValue> term = GetTermSetFromDataTableRow(dataTable.Columns, dataTable.Rows[rowIndex]);

                foreach (FactorFuzzyValue t in term)
                    termSet.Add(t);
            }

            return termSet;
        }
        private IList<FactorFuzzyValue> GetTermSetFromDataTableRow(DataColumnCollection columns, DataRow row)
        {
            Dictionary<string, object> data = new Dictionary<string, object>();
            for (int col = 0; col < columns.Count; col++)
                data.Add(columns[col].ColumnName, row[col]);

            IList<FactorFuzzyValue> termSet = GetTermSet(data["TermSet"].ToString());

            return termSet;
        }
        #endregion

        #region Methods converting Json to list<string>/dictionary<T, List<string>>
        private Dictionary<FactorFuzzyValue, IList<string>> GetDictionaryValuesFromJson(string json, IList<FactorFuzzyValue> termSet)
        {
            Dictionary<FactorFuzzyValue, IList<string>> values = new Dictionary<FactorFuzzyValue, IList<string>>();
            foreach (FactorFuzzyValue term in termSet)
                values.Add(term, new List<string>());

            string[] jsonLines = Regex.Split(json, "],");
            IEnumerator<FactorFuzzyValue> termSetEnum = termSet.GetEnumerator();
            foreach (string jsonLine in jsonLines)
            {
                while (!jsonLine.Contains(termSetEnum.Current.ToString()))
                    termSetEnum.MoveNext();

                IList<string> result = GetListValuesFromJson(jsonLine, termSetEnum.Current.ToString());
                if (result.Count == 1)
                {
                    values[termSetEnum.Current] = values[termSetEnum.Current] ?? new List<string>();
                    values[termSetEnum.Current].Add(result.First());
                }
                else
                {
                    values[termSetEnum.Current] = values[termSetEnum.Current] ?? new List<string>();
                    foreach (string line in result)
                        values[termSetEnum.Current].Add(line);
                }
            }

            return values;
        }
        private IList<string> GetListValuesFromJson(string jsonLine, string attribute)
        {
            IList<string> values = new List<string>();

            if (jsonLine != "")
            {
                List<string> words = new List<string>();

                jsonLine = jsonLine.Trim(new char[] { '{', '}' });
                string[] symbolWords = Regex.Split(jsonLine, "\"");

                for (int i = 0; i < symbolWords.Length; i++)
                {
                    symbolWords[i] = symbolWords[i].Trim(' ');
                    if (!symbolWords[i].Contains("[") && !symbolWords[i].Contains("]") &&
                        !symbolWords[i].Contains("{") && !symbolWords[i].Contains("}") &&
                        !symbolWords[i].Contains(":") && symbolWords[i] != "" &&
                        symbolWords[i] != "," && symbolWords[i] != ".")
                        words.Add(symbolWords[i].Replace(',', '.'));
                }

                for (int i = 0; i < words.Count; i++)
                {
                    if (words[i] == attribute)
                    {
                        for (int j = 1; j < words.Count; j++)
                            values.Add(words[j]);
                    }
                    break;
                }
            }

            return values;
        }
        #endregion

        #region If there is no connection to db
        private IList<ILinguisticVariable> GetStaticLinguisticVariables()
        {
            return new List<ILinguisticVariable>
            {
                new LinguisticVariable(
                    FactorTitle.dt,
                    new Dictionary<FactorFuzzyValue, decimal[]>
                    {
                        [FactorFuzzyValue.Successful] = new decimal[2] { 0m, 30m },
                        [FactorFuzzyValue.Unsuccessful] = new decimal[2] { 0m, 10000m },
                    },
                    new List<FactorFuzzyValue> { FactorFuzzyValue.Unsuccessful, FactorFuzzyValue.Successful },
                    "Длительность провала напряжения",
                    "сек."
                    ),
                new LinguisticVariable(
                    FactorTitle.K2U,
                    new Dictionary<FactorFuzzyValue, decimal[]>
                    {
                        [FactorFuzzyValue.Nominal] = new decimal[2] { 0m, 2m },
                        [FactorFuzzyValue.Maximum] = new decimal[2] { 0m, 4m },
                        [FactorFuzzyValue.Exceeding] = new decimal[2] { 0m, 10000m },
                    },
                    new List<FactorFuzzyValue> { FactorFuzzyValue.Nominal, FactorFuzzyValue.Maximum, FactorFuzzyValue.Exceeding },
                    "Коэффициент несимметрии напряжений по обратной последовательности",
                    "%"
                    ),
                new LinguisticVariable(
                    FactorTitle.K2Ui,
                    null,
                    new List<FactorFuzzyValue> { FactorFuzzyValue.Unsuccessful, FactorFuzzyValue.Successful },
                    "Несимметрия напряжений",
                    "%"
                    ),
                new LinguisticVariable(
                    FactorTitle.KUn,
                    new Dictionary<FactorFuzzyValue, decimal[]>
                    {
                        [FactorFuzzyValue.Nominal] = new decimal[2] { 1m, 1.5m },
                        [FactorFuzzyValue.Maximum] = new decimal[2] { 0.7m, 1.8m },
                        [FactorFuzzyValue.Exceeding] = new decimal[2] { 0m, 10000m },
                    },
                    new List<FactorFuzzyValue> { FactorFuzzyValue.Nominal, FactorFuzzyValue.Maximum, FactorFuzzyValue.Exceeding },
                    "Коэффициент n-ой гармонической составляющей напряжения",
                    "%"
                    ),
                new LinguisticVariable(
                    FactorTitle.Weather,
                    null,
                    new List<FactorFuzzyValue> { FactorFuzzyValue.Unsuccessful, FactorFuzzyValue.Successful },
                    "Погодные условия",
                    null
                    ),
                new LinguisticVariable(
                    FactorTitle.Ua,
                    new Dictionary<FactorFuzzyValue, decimal[]>
                    {
                        [FactorFuzzyValue.Nominal] = new decimal[2] { 209m, 231m },
                        [FactorFuzzyValue.Maximum] = new decimal[2] { 198m, 242m },
                        [FactorFuzzyValue.Exceeding] = new decimal[2] { 0m, 10000m },
                    },
                    new List<FactorFuzzyValue> { FactorFuzzyValue.Nominal, FactorFuzzyValue.Maximum, FactorFuzzyValue.Exceeding },
                    "Напряжения по фазе A",
                    "В"
                    ),
                new LinguisticVariable(
                    FactorTitle.Ub,
                    new Dictionary<FactorFuzzyValue, decimal[]>
                    {
                        [FactorFuzzyValue.Nominal] = new decimal[2] { 209m, 231m },
                        [FactorFuzzyValue.Maximum] = new decimal[2] { 198m, 242m },
                        [FactorFuzzyValue.Exceeding] = new decimal[2] { 0m, 10000m },
                    },
                    new List<FactorFuzzyValue> { FactorFuzzyValue.Nominal, FactorFuzzyValue.Maximum, FactorFuzzyValue.Exceeding },
                    "Напряжения по фазе B",
                    "В"
                    ),
                new LinguisticVariable(
                    FactorTitle.Uc,
                    new Dictionary<FactorFuzzyValue, decimal[]>
                    {
                        [FactorFuzzyValue.Nominal] = new decimal[2] { 209m, 231m },
                        [FactorFuzzyValue.Maximum] = new decimal[2] { 198m, 242m },
                        [FactorFuzzyValue.Exceeding] = new decimal[2] { 0m, 10000m },
                    },
                    new List<FactorFuzzyValue> { FactorFuzzyValue.Nominal, FactorFuzzyValue.Maximum, FactorFuzzyValue.Exceeding },
                    "Напряжения по фазе C",
                    "В"
                    ),
                new LinguisticVariable(
                    FactorTitle.Uimp,
                    null,
                    new List<FactorFuzzyValue> { FactorFuzzyValue.Nominal, FactorFuzzyValue.Maximum, FactorFuzzyValue.Exceeding },
                    "Грозовые импульсные напряжения",
                    "В"
                    ),
                new LinguisticVariable(
                    FactorTitle.dUabc,
                    null,
                    new List<FactorFuzzyValue> { FactorFuzzyValue.Nominal, FactorFuzzyValue.Maximum, FactorFuzzyValue.Exceeding },
                    "Отклонение напряжения по фазам А, В, С",
                    "%"
                    ),
                new LinguisticVariable(
                    FactorTitle.KU,
                    null,
                    new List<FactorFuzzyValue> { FactorFuzzyValue.Nominal, FactorFuzzyValue.Maximum, FactorFuzzyValue.Exceeding },
                    "Коэффициент искажения синусоидальности кривой напряжения",
                    "%"
                    ),
                new LinguisticVariable(
                    FactorTitle.Uns,
                    null,
                    new List<FactorFuzzyValue> { FactorFuzzyValue.Nominal, FactorFuzzyValue.Maximum, FactorFuzzyValue.Exceeding },
                    "Несинусоидальность напряжения",
                    "В"
                    ),
                new LinguisticVariable(
                    FactorTitle.ServiceabilityEquipment,
                    null,
                    new List<FactorFuzzyValue> { FactorFuzzyValue.Nominal, FactorFuzzyValue.Maximum, FactorFuzzyValue.Exceeding },
                    "Исправность технического состояния оборудования",
                    null
                    )
            };
        }
        private IList<ILinguisticVariable> GetStaticLinguisticVariables(IList<IParameter> parameters)
        {
            return GetStaticLinguisticVariables().Where(lv => parameters.Select(p => p.Title).Contains(lv.Title)).ToList();
        }
        #region GetStaticRules
        private IList<KB.Rule> GetStaticRules()
        {
            return new List<KB.Rule>
            {
                GetRule01(),
                GetRule02(),
                GetRule03(),
                GetRule11(),
                GetRule12(),
                GetRule13(),
                GetRule14(),
                GetRule21(),
                GetRule22(),
                GetRule23(),
                GetRule31(),
                GetRule32(),
                GetRule33(),
                GetRule41(),
                GetRule42(),
                GetRule51(),
                GetRule52(),
                GetRule53(),
            };
        }

        #region ServiceabilityEquipment
        private KB.Rule GetRule01()
        {
            Antecedent antecedent;
            Judgment consequent;

            antecedent = new Antecedent()
                .OR(new Judgment(FactorTitle.Uimp, FactorFuzzyValue.Exceeding), new Judgment(FactorTitle.dUabc, FactorFuzzyValue.Exceeding), new Judgment(FactorTitle.Uns, FactorFuzzyValue.Exceeding), new Judgment(FactorTitle.K2U, FactorFuzzyValue.Exceeding))
                .OR(new Judgment(FactorTitle.K2Ui, FactorFuzzyValue.Successful), new Judgment(FactorTitle.K2Ui, FactorFuzzyValue.Unsuccessful))
                .OR(new Judgment(FactorTitle.dt, FactorFuzzyValue.Successful), new Judgment(FactorTitle.dt, FactorFuzzyValue.Unsuccessful));
            consequent = new Judgment(FactorTitle.ServiceabilityEquipment, FactorFuzzyValue.Exceeding);

            return new SimpleRule(linguisticVariable, antecedent, consequent);
        }
        private KB.Rule GetRule02()
        {
            Antecedent antecedent;
            Judgment consequent;

            antecedent = new Antecedent()
                .OR(new Judgment(FactorTitle.Uimp, FactorFuzzyValue.Maximum), new Judgment(FactorTitle.dUabc, FactorFuzzyValue.Maximum), new Judgment(FactorTitle.Uns, FactorFuzzyValue.Maximum), new Judgment(FactorTitle.K2U, FactorFuzzyValue.Maximum), new Judgment(FactorTitle.K2Ui, FactorFuzzyValue.Unsuccessful), new Judgment(FactorTitle.dt, FactorFuzzyValue.Unsuccessful))
                .OR(new Judgment(FactorTitle.Uimp, FactorFuzzyValue.Maximum), new Judgment(FactorTitle.Uimp, FactorFuzzyValue.Nominal))
                .OR(new Judgment(FactorTitle.dUabc, FactorFuzzyValue.Maximum), new Judgment(FactorTitle.dUabc, FactorFuzzyValue.Nominal))
                .OR(new Judgment(FactorTitle.Uns, FactorFuzzyValue.Maximum), new Judgment(FactorTitle.Uns, FactorFuzzyValue.Nominal))
                .OR(new Judgment(FactorTitle.K2U, FactorFuzzyValue.Maximum), new Judgment(FactorTitle.K2U, FactorFuzzyValue.Nominal))
                .OR(new Judgment(FactorTitle.K2Ui, FactorFuzzyValue.Successful), new Judgment(FactorTitle.K2Ui, FactorFuzzyValue.Unsuccessful))
                .OR(new Judgment(FactorTitle.dt, FactorFuzzyValue.Successful), new Judgment(FactorTitle.dt, FactorFuzzyValue.Unsuccessful));
            consequent = new Judgment(FactorTitle.ServiceabilityEquipment, FactorFuzzyValue.Maximum);

            return new SimpleRule(linguisticVariable, antecedent, consequent);
        }
        private KB.Rule GetRule03()
        {
            Antecedent antecedent;
            Judgment consequent;

            antecedent = new Antecedent()
                .OR(new Judgment(FactorTitle.Uimp, FactorFuzzyValue.Nominal))
                .AND(new Judgment(FactorTitle.dUabc, FactorFuzzyValue.Nominal))
                .AND(new Judgment(FactorTitle.Uns, FactorFuzzyValue.Nominal))
                .AND(new Judgment(FactorTitle.K2U, FactorFuzzyValue.Nominal))
                .AND(new Judgment(FactorTitle.K2Ui, FactorFuzzyValue.Successful))
                .AND(new Judgment(FactorTitle.dt, FactorFuzzyValue.Successful));
            consequent = new Judgment(FactorTitle.ServiceabilityEquipment, FactorFuzzyValue.Nominal);

            return new SimpleRule(linguisticVariable, antecedent, consequent);
        }
        #endregion

        #region Uimp
        private KB.Rule GetRule11()
        {
            Antecedent antecedent;
            Judgment consequent;

            antecedent = new Antecedent()
                .OR(new Judgment(FactorTitle.Ua, FactorFuzzyValue.Exceeding), new Judgment(FactorTitle.Ub, FactorFuzzyValue.Exceeding), new Judgment(FactorTitle.Uc, FactorFuzzyValue.Exceeding))
                .AND(new Judgment(FactorTitle.Weather, FactorFuzzyValue.Unsuccessful));
            consequent = new Judgment(FactorTitle.Uimp, FactorFuzzyValue.Exceeding);

            return new SimpleRule(linguisticVariable, antecedent, consequent);
        }
        private KB.Rule GetRule12()
        {
            Antecedent antecedent;
            Judgment consequent;
            Judgment other;

            antecedent = new Antecedent()
                .OR(new Judgment(FactorTitle.Ua, FactorFuzzyValue.Maximum), new Judgment(FactorTitle.Ub, FactorFuzzyValue.Maximum), new Judgment(FactorTitle.Uc, FactorFuzzyValue.Maximum))
                .OR(new Judgment(FactorTitle.Ua, FactorFuzzyValue.Maximum), new Judgment(FactorTitle.Ua, FactorFuzzyValue.Nominal))
                .OR(new Judgment(FactorTitle.Ub, FactorFuzzyValue.Maximum), new Judgment(FactorTitle.Ua, FactorFuzzyValue.Nominal))
                .OR(new Judgment(FactorTitle.Uc, FactorFuzzyValue.Maximum), new Judgment(FactorTitle.Ua, FactorFuzzyValue.Nominal))
                .AND(new Judgment(FactorTitle.Weather, FactorFuzzyValue.Unsuccessful));
            consequent = new Judgment(FactorTitle.Uimp, FactorFuzzyValue.Maximum);

            return new SimpleRule(linguisticVariable, antecedent, consequent);
        }
        private KB.Rule GetRule13()
        {
            Antecedent antecedent;
            Judgment consequent;

            antecedent = new Antecedent()
                .OR(new Judgment(FactorTitle.Ua, FactorFuzzyValue.Nominal))
                .AND(new Judgment(FactorTitle.Ub, FactorFuzzyValue.Nominal))
                .AND(new Judgment(FactorTitle.Uc, FactorFuzzyValue.Nominal))
                .AND(new Judgment(FactorTitle.Weather, FactorFuzzyValue.Unsuccessful));
            consequent = new Judgment(FactorTitle.Uimp, FactorFuzzyValue.Nominal);

            return new SimpleRule(linguisticVariable, antecedent, consequent);
        }
        private KB.Rule GetRule14()
        {
            Antecedent antecedent;
            Judgment consequent;

            antecedent = new Antecedent()
                .AND(new Judgment(FactorTitle.Weather, FactorFuzzyValue.Successful));
            consequent = new Judgment(FactorTitle.Uimp, FactorFuzzyValue.Nominal);

            return new SimpleRule(linguisticVariable, antecedent, consequent);
        }
        #endregion

        #region dUabc
        private KB.Rule GetRule21()
        {
            Antecedent antecedent;
            Judgment consequent;

            antecedent = new Antecedent()
                .OR(new Judgment(FactorTitle.Ua, FactorFuzzyValue.Exceeding), new Judgment(FactorTitle.Ub, FactorFuzzyValue.Exceeding), new Judgment(FactorTitle.Uc, FactorFuzzyValue.Exceeding));
            consequent = new Judgment(FactorTitle.dUabc, FactorFuzzyValue.Exceeding);

            return new SimpleRule(linguisticVariable, antecedent, consequent);
        }
        private KB.Rule GetRule22()
        {
            Antecedent antecedent;
            Judgment consequent;

            antecedent = new Antecedent()
                .OR(new Judgment(FactorTitle.Ua, FactorFuzzyValue.Maximum), new Judgment(FactorTitle.Ub, FactorFuzzyValue.Maximum), new Judgment(FactorTitle.Uc, FactorFuzzyValue.Maximum))
                .OR(new Judgment(FactorTitle.Ua, FactorFuzzyValue.Maximum), new Judgment(FactorTitle.Ua, FactorFuzzyValue.Nominal))
                .OR(new Judgment(FactorTitle.Ub, FactorFuzzyValue.Maximum), new Judgment(FactorTitle.Ua, FactorFuzzyValue.Nominal))
                .OR(new Judgment(FactorTitle.Uc, FactorFuzzyValue.Maximum), new Judgment(FactorTitle.Ua, FactorFuzzyValue.Nominal));
            consequent = new Judgment(FactorTitle.dUabc, FactorFuzzyValue.Maximum);

            return new SimpleRule(linguisticVariable, antecedent, consequent);
        }
        private KB.Rule GetRule23()
        {
            Antecedent antecedent;
            Judgment consequent;

            antecedent = new Antecedent()
                .OR(new Judgment(FactorTitle.Ua, FactorFuzzyValue.Nominal))
                .AND(new Judgment(FactorTitle.Ub, FactorFuzzyValue.Nominal))
                .AND(new Judgment(FactorTitle.Uc, FactorFuzzyValue.Nominal));
            consequent = new Judgment(FactorTitle.dUabc, FactorFuzzyValue.Nominal);

            return new SimpleRule(linguisticVariable, antecedent, consequent);
        }
        #endregion

        #region KU
        private KB.Rule GetRule31()
        {
            Antecedent antecedent;
            Judgment consequent;

            antecedent = new Antecedent()
                .OR(new Judgment(FactorTitle.Ua, FactorFuzzyValue.Exceeding), new Judgment(FactorTitle.Ub, FactorFuzzyValue.Exceeding), new Judgment(FactorTitle.Uc, FactorFuzzyValue.Exceeding));
            consequent = new Judgment(FactorTitle.KU, FactorFuzzyValue.Exceeding);

            return new SimpleRule(linguisticVariable, antecedent, consequent);
        }
        private KB.Rule GetRule32()
        {
            Antecedent antecedent;
            Judgment consequent;

            antecedent = new Antecedent()
                .OR(new Judgment(FactorTitle.Ua, FactorFuzzyValue.Maximum), new Judgment(FactorTitle.Ub, FactorFuzzyValue.Maximum), new Judgment(FactorTitle.Uc, FactorFuzzyValue.Maximum))
                .OR(new Judgment(FactorTitle.Ua, FactorFuzzyValue.Maximum), new Judgment(FactorTitle.Ua, FactorFuzzyValue.Nominal))
                .OR(new Judgment(FactorTitle.Ub, FactorFuzzyValue.Maximum), new Judgment(FactorTitle.Ua, FactorFuzzyValue.Nominal))
                .OR(new Judgment(FactorTitle.Uc, FactorFuzzyValue.Maximum), new Judgment(FactorTitle.Ua, FactorFuzzyValue.Nominal));
            consequent = new Judgment(FactorTitle.KU, FactorFuzzyValue.Maximum);

            return new SimpleRule(linguisticVariable, antecedent, consequent);
        }
        private KB.Rule GetRule33()
        {
            Antecedent antecedent;
            Judgment consequent;

            antecedent = new Antecedent()
                .OR(new Judgment(FactorTitle.Ua, FactorFuzzyValue.Nominal))
                .AND(new Judgment(FactorTitle.Ub, FactorFuzzyValue.Nominal))
                .AND(new Judgment(FactorTitle.Uc, FactorFuzzyValue.Nominal));
            consequent = new Judgment(FactorTitle.KU, FactorFuzzyValue.Nominal);

            return new SimpleRule(linguisticVariable, antecedent, consequent);
        }
        #endregion

        #region K2Ui
        private KB.Rule GetRule41()
        {
            Antecedent antecedent;
            Judgment consequent;

            antecedent = new Antecedent()
                .AND(new Judgment(FactorTitle.K2U, FactorFuzzyValue.Nominal));
            consequent = new Judgment(FactorTitle.K2Ui, FactorFuzzyValue.Successful);

            return new SimpleRule(linguisticVariable, antecedent, consequent);
        }
        private KB.Rule GetRule42()
        {
            Antecedent antecedent;
            Judgment consequent;

            antecedent = new Antecedent()
                .OR(new Judgment(FactorTitle.K2U, FactorFuzzyValue.Exceeding), new Judgment(FactorTitle.K2U, FactorFuzzyValue.Maximum));
            consequent = new Judgment(FactorTitle.K2Ui, FactorFuzzyValue.Unsuccessful);

            return new SimpleRule(linguisticVariable, antecedent, consequent);
        }
        #endregion

        #region Uns
        private KB.Rule GetRule51()
        {
            Antecedent antecedent;
            Judgment consequent;

            antecedent = new Antecedent()
                .OR(new Judgment(FactorTitle.KU, FactorFuzzyValue.Exceeding), new Judgment(FactorTitle.KUn, FactorFuzzyValue.Exceeding));
            consequent = new Judgment(FactorTitle.Uns, FactorFuzzyValue.Exceeding);

            return new SimpleRule(linguisticVariable, antecedent, consequent);
        }
        private KB.Rule GetRule52()
        {
            Antecedent antecedent;
            Judgment consequent;

            antecedent = new Antecedent()
                .OR(new Judgment(FactorTitle.KU, FactorFuzzyValue.Maximum), new Judgment(FactorTitle.KUn, FactorFuzzyValue.Maximum))
                .OR(new Judgment(FactorTitle.KU, FactorFuzzyValue.Maximum), new Judgment(FactorTitle.KUn, FactorFuzzyValue.Nominal))
                .OR(new Judgment(FactorTitle.KUn, FactorFuzzyValue.Maximum), new Judgment(FactorTitle.KUn, FactorFuzzyValue.Nominal));
            consequent = new Judgment(FactorTitle.Uns, FactorFuzzyValue.Maximum);

            return new SimpleRule(linguisticVariable, antecedent, consequent);
        }
        private KB.Rule GetRule53()
        {
            Antecedent antecedent;
            Judgment consequent;

            antecedent = new Antecedent()
                .AND(new Judgment(FactorTitle.KU, FactorFuzzyValue.Nominal))
                .AND(new Judgment(FactorTitle.KUn, FactorFuzzyValue.Nominal));
            consequent = new Judgment(FactorTitle.Uns, FactorFuzzyValue.Nominal);

            return new SimpleRule(linguisticVariable, antecedent, consequent);
        }
        #endregion

        #endregion
        private IList<FactorFuzzyValue> GetStaticTermSets()
        {
            return new List<FactorFuzzyValue>
            {
                FactorFuzzyValue.Unsuccessful,
                FactorFuzzyValue.Successful,
                FactorFuzzyValue.Nominal,
                FactorFuzzyValue.Maximum,
                FactorFuzzyValue.Exceeding
            };
            #endregion
        }
    }
}