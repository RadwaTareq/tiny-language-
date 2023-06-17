using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JASON_Compiler
{
    public class Node
    {
        public List<Node> Children = new List<Node>();
        
        public string Name;
        public Node(string N)
        {
            this.Name = N;
        }
    }
    public class Parser
    {
        int InputPointer = 0;
        List<Token> TokenStream;
        public  Node root;
        
        public Node StartParsing(List<Token> TokenStream)
        {
            this.InputPointer = 0;
            this.TokenStream = TokenStream;
            root = new Node("Program");
            root.Children.Add(Program());
            MessageBox.Show("Success");
            return root;
        }
        Node Program()
        {
            Node program = new Node("Program Function");
            if(InputPointer<TokenStream.Count&&TokenStream[InputPointer+1].token_type!=Token_Class.MAIN)
            {
                program.Children.Add(Function_Statement());
                program.Children.Add(Program());
            }
            else
                program.Children.Add(Main_Function());

            return program;
        }
        // Implement your logic here
        //nada
        Node function_call()
        {
            Node fnode = new Node("function_call");

            if (InputPointer < TokenStream.Count)
            {
                if (Token_Class.Idenifier == TokenStream[InputPointer].token_type)
                {
                    fnode.Children.Add(match(Token_Class.Idenifier));
                    fnode.Children.Add(match(Token_Class.LParanthesis));
                    fnode.Children.Add(identifiers());
                    fnode.Children.Add(match(Token_Class.RParanthesis));
                }
            }
            return fnode;
        }
        public Node identifiers()
        {
            Node node = new Node("identifiers");
            if (InputPointer < TokenStream.Count)
            {
                if (Token_Class.Idenifier == TokenStream[InputPointer].token_type)
                {
                    node.Children.Add(match(Token_Class.Idenifier));
                    node.Children.Add(id());
                    return node;
                }
                else
                    return null;
            }
            return null;
        }
        public Node id()
        {
            Node inode = new Node("id");
            if (InputPointer < TokenStream.Count)
            {
                if (Token_Class.Comma == TokenStream[InputPointer].token_type)
                {
                    inode.Children.Add(match(Token_Class.Comma));
                    inode.Children.Add(match(Token_Class.Idenifier));
                    inode.Children.Add(id());
                    return inode;
                }
                else
                    return null;
            }
            return null;

        }
        public Node Term()
        {
            Node tnode = new Node("Term");
            if (InputPointer < TokenStream.Count)
            {
                if (Token_Class.Number == TokenStream[InputPointer].token_type)
                {
                    tnode.Children.Add(match(Token_Class.Number));
                }
                else if (Token_Class.Idenifier == TokenStream[InputPointer].token_type)
                {
                    //change it
                    if (InputPointer + 1 < TokenStream.Count)
                    {
                        if (Token_Class.LParanthesis != TokenStream[InputPointer + 1].token_type)
                        {
                            tnode.Children.Add(match(Token_Class.Idenifier));
                        }
                        else
                            tnode.Children.Add(function_call());

                    }
                    else
                        tnode.Children.Add(match(Token_Class.Idenifier));

                }



            }

            return tnode;
        }
        //change it
        public Node Term_()
        {
            Node node = new Node("Term_");
            if (InputPointer < TokenStream.Count)
            {
                if (Token_Class.LParanthesis == TokenStream[InputPointer].token_type)
                {
                    node.Children.Add(match(Token_Class.LParanthesis));
                    node.Children.Add(identifiers());
                    node.Children.Add(match(Token_Class.RParanthesis));
                    return node;
                }
                else
                    return null;
            }

            return null;
        }
        Node equation()
        {
            Node node = new Node("equation");


            if (InputPointer < TokenStream.Count)
            {
                Token_Class AO = Arithmatic_Operator(TokenStream[InputPointer].token_type);
                if (Token_Class.LParanthesis == TokenStream[InputPointer].token_type)
                {
                    node.Children.Add(match(Token_Class.LParanthesis));
                    node.Children.Add(Term());
                    node.Children.Add(match(AO));
                    node.Children.Add(equation());
                    node.Children.Add(match(Token_Class.RParanthesis));
                    node.Children.Add(equation_());


                }
                else
                {
                    node.Children.Add(Term());
                    node.Children.Add(equation_());
                }

            }
            return node;
        }
        Node equation_()
        {
            Node node = new Node("equation_");


            if (InputPointer < TokenStream.Count)
            {
                Token_Class AO = Arithmatic_Operator(TokenStream[InputPointer].token_type);
                if (AO == TokenStream[InputPointer].token_type)
                {
                    node.Children.Add(match(AO));
                    node.Children.Add(equation());
                    return node;
                }
                else
                    return null;

            }
            return null;
        }
        Token_Class Arithmatic_Operator(Token_Class token)
        {
            Token_Class res = new Token_Class();
            if (token == Token_Class.PlusOp)
            {
                res = Token_Class.PlusOp;
            }
            else if (token == Token_Class.MinusOp)
            {
                res = Token_Class.MinusOp;
            }
            else if (token == Token_Class.DivideOp)
            {
                res = Token_Class.DivideOp;
            }
            else if (token == Token_Class.MultiplyOp)
            {
                res = Token_Class.MultiplyOp;
            }
            else
                return Token_Class.PlusOp; //here 


            return res;


        }

        public Node expression()
        {
            Node enode = new Node("Expression");

            if (InputPointer < TokenStream.Count)
            {
                //change it from String token TO STRING Token
                if (Token_Class.STRING == TokenStream[InputPointer].token_type)
                {
                    enode.Children.Add(match(Token_Class.STRING));
                }
                else if (Token_Class.LParanthesis == TokenStream[InputPointer].token_type)
                {
                    enode.Children.Add(equation());
                }
                else
                {
                    enode.Children.Add(Term());
                    enode.Children.Add(equation_());
                }


            }

            return enode;
        }
        //nada
        private Node Date_Type()
        {
            Node node = new Node("Date Type");
            if (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.INT)
            {
                node.Children.Add(match(Token_Class.INT));
                return node;
            }
            else if (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.FLOAT)
            {
                node.Children.Add(match(Token_Class.FLOAT));
                return node;
            }
            else if (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.STRING)
            {
                node.Children.Add(match(Token_Class.STRING));
                return node;
            }
            return node;
        }
        //youssef
        Node VarDecl()
        {
            Node node = new Node("VarDecl");
            if (InputPointer < TokenStream.Capacity)
            {
                node.Children.Add(Date_Type());
                node.Children.Add(IdList());
                return node;
            }
            return node;
        }
        Node IdList()
        {
            Node node = new Node("IdList");
            if (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.Idenifier)
            {
                node.Children.Add(match(Token_Class.Idenifier));
                if (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.Assign)
                {
                    node.Children.Add(match(Token_Class.Assign));
                    node.Children.Add(expression());
                    node.Children.Add(IdList_());
                }
                else
                {
                    node.Children.Add(IdList_());
                }

                return node;
            }

            return node;
        }
        Node IdList_()
        {
            Node node = new Node("IdList_");
            if (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.Comma)
            {
                node.Children.Add(match(Token_Class.Comma));
                node.Children.Add(match(Token_Class.Idenifier));
                if (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.Assign)
                {
                    node.Children.Add(match(Token_Class.Assign));
                    node.Children.Add(expression());
                    node.Children.Add(IdList_());
                }
                else
                    node.Children.Add(IdList_());
                return node;
            }
            else
                return null;

            return node;
        }
        //youssef
        //radwa
        public Node BooleanOp()
        {
            Node BooleanOp = new Node("Boolean Operation");
            if (TokenStream[InputPointer].token_type == Token_Class.AndOp)
            {
                BooleanOp.Children.Add(match(Token_Class.AndOp));
            }
            else if (TokenStream[InputPointer].token_type == Token_Class.OROp)
            {
                BooleanOp.Children.Add(match(Token_Class.OROp));
            }
            return BooleanOp;
        }
        public Node ConditionOp()
        {
            Node conditionOp = new Node("Condition Operation");
            if (TokenStream[InputPointer].token_type == Token_Class.EqualOp)
            {
                conditionOp.Children.Add(match(Token_Class.EqualOp));
            }
            else if (TokenStream[InputPointer].token_type == Token_Class.NotEqual)
            {
                conditionOp.Children.Add(match(Token_Class.NotEqual));
            }
            else if (TokenStream[InputPointer].token_type == Token_Class.GreaterThanOp)
            {
                conditionOp.Children.Add(match(Token_Class.GreaterThanOp));
            }
            else if (TokenStream[InputPointer].token_type == Token_Class.LessThanOp)
            {
                conditionOp.Children.Add(match(Token_Class.LessThanOp));
            }
            return conditionOp;



        }
        public Node Condition_()
        {
            Node Condition_ = new Node("condition_");
            if (InputPointer < TokenStream.Count)
            {
                if (TokenStream[InputPointer].token_type == Token_Class.OROp || TokenStream[InputPointer].token_type == Token_Class.AndOp)
                {
                    Condition_.Children.Add(BooleanOp());
                    Condition_.Children.Add(Condition());
                }
            }
            else
            {
                return null;
            }

            return Condition_;
        }
        public Node Condition()
        {
            Node Condition = new Node("condition");
            if (InputPointer < TokenStream.Count)
            {
                Condition.Children.Add(match(Token_Class.Idenifier));

                Condition.Children.Add(ConditionOp());
                Condition.Children.Add(Term());
                Condition.Children.Add(Condition_());
            }
            return Condition;
        }
        public Node else_statement()
        {
            Node else_statement = new Node("Else_statement");
            else_statement.Children.Add(match(Token_Class.ELSE));
            else_statement.Children.Add(Statements());
            else_statement.Children.Add(match(Token_Class.END));

            return else_statement;
        }
        public Node elseif_statement()
        {
            Node elseif_statement = new Node("Elseif_statement");
            elseif_statement.Children.Add(match(Token_Class.ELESIF));
            elseif_statement.Children.Add(Condition());
            elseif_statement.Children.Add(match(Token_Class.THEN));
            elseif_statement.Children.Add(Statements());
            elseif_statement.Children.Add(E());

            return elseif_statement;
        }
        public Node E()
        {
            Node E = new Node("E");
            if (InputPointer < TokenStream.Count)
            {
                if (TokenStream[InputPointer].token_type == Token_Class.ELESIF)
                {
                    E.Children.Add(elseif_statement());
                }
                else if (TokenStream[InputPointer].token_type == Token_Class.ELSE)
                {
                    E.Children.Add(else_statement());
                }
                else
                {
                    E.Children.Add(match(Token_Class.END));
                }
            }
            return E;
        }
        public Node if_statement()
        {
            Node if_statement = new Node("IF_Statement");
            if_statement.Children.Add(match(Token_Class.IF));
            if_statement.Children.Add(Condition());
            if_statement.Children.Add(match(Token_Class.THEN));
            if_statement.Children.Add(Statements());
            if_statement.Children.Add(E());
            return if_statement;

        }
        //radwa
        //hassna
        public Node Function_Name()
        {
            Node function_name = new Node("Function Name");

            if (Token_Class.Idenifier == TokenStream[InputPointer].token_type)
            {
                function_name.Children.Add(match(Token_Class.Idenifier));
                return function_name;
            }
            else
                return null;


        }
        public Node Parameter()
        {
            Node parameter = new Node("Parameter");

            if (InputPointer < TokenStream.Count)
            {
                parameter.Children.Add(Date_Type());
                parameter.Children.Add(match(Token_Class.Idenifier));
                return parameter;
            }
            else
                return null;


        }
        public Node Parameter_()
        {
            Node parameter_ = new Node("Parameter_");

            if (Token_Class.Comma == TokenStream[InputPointer].token_type)
            {
                parameter_.Children.Add(match(Token_Class.Comma));
                parameter_.Children.Add(Parameter());
                return parameter_;
            }
            else
                return null;


        }
        public Node Repeat_statement()
        {
            Node repeat_statement = new Node("Repeat Statement");

            if (Token_Class.REPEAT == TokenStream[InputPointer].token_type)
            {
                repeat_statement.Children.Add(match(Token_Class.REPEAT));
                repeat_statement.Children.Add(Statements());
                repeat_statement.Children.Add(match(Token_Class.UNTIL));
                repeat_statement.Children.Add(Condition());
                return repeat_statement;
            }
            else
                return null;


        }

        public Node Function_Declaration()
        {
            Node function_declaration = new Node("Function Declaration");
            if (InputPointer < TokenStream.Count)
            {


                function_declaration.Children.Add(Date_Type());
                function_declaration.Children.Add(Function_Name());
                function_declaration.Children.Add(match(Token_Class.LParanthesis));
                function_declaration.Children.Add(Parameter());
                function_declaration.Children.Add(Parameter_());
                function_declaration.Children.Add(match(Token_Class.RParanthesis));
                return function_declaration;

            }
            else
                return null;


        }
        //hassna
        //mohamady
        private Node Main_Function()
        {
            Node main = new Node("Main Function");
            main.Children.Add(Date_Type());
            main.Children.Add(match(Token_Class.MAIN));
            main.Children.Add(match(Token_Class.LParanthesis));
            main.Children.Add(match(Token_Class.RParanthesis));
            main.Children.Add(Function_Body());
            return main;
        }

        private Node Function_Body()
        {
            Node node = new Node("Function Body");
            node.Children.Add(match(Token_Class.LBraces));
            node.Children.Add(Statements());
            //node.Children.Add(Return_Statement());
            node.Children.Add(match(Token_Class.RBraces));
            return node;
        }
        private Node Statements()
        {
            Node node = new Node("Statements");
            if (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.IF)
            {
                node.Children.Add(if_statement());
                node.Children.Add(Statements());
            }
            else if (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.REPEAT)
            {
                node.Children.Add(Repeat_statement());
                node.Children.Add(Statements());
            }
            else if(InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.COMMENT)
            {
                node.Children.Add(match(Token_Class.COMMENT));
                node.Children.Add(Statements());
            }
            else if (InputPointer < TokenStream.Count && check_Statement())
            {
                node.Children.Add(Statement());
                node.Children.Add(match(Token_Class.Semicolon));
                node.Children.Add(Statements());
            }
            else
            {
                return null;
            }
            return node;
        }
        private bool check_Statement()
        {
            if (TokenStream[InputPointer].token_type == Token_Class.READ || TokenStream[InputPointer].token_type == Token_Class.WRITE ||
                TokenStream[InputPointer].token_type == Token_Class.STRING || TokenStream[InputPointer].token_type == Token_Class.FLOAT ||
                TokenStream[InputPointer].token_type == Token_Class.INT || TokenStream[InputPointer].token_type == Token_Class.RETURN ||
                TokenStream[InputPointer].token_type == Token_Class.Idenifier)
            {
                return true;
            }
            return false;
        }
        private Node Return_Statement()
        {
            Node node = new Node("Return Statement");
            node.Children.Add(match(Token_Class.RETURN));
            node.Children.Add(expression());
            return node;
        }

        private Node Function_Statement()
        {
            Node node = new Node("Function Statement");
            node.Children.Add(Function_Declaration());
            node.Children.Add(Function_Body());
            return node;
        }
        Token_Class Ao(Token_Class pstream)
        {
            Token_Class res = new Token_Class();
            if (pstream == Token_Class.PlusOp)
                res = Token_Class.PlusOp;
            else if (pstream == Token_Class.MinusOp)
                res = Token_Class.MinusOp;
            else if (pstream == Token_Class.DivideOp)
                res = Token_Class.DivideOp;
            else if (pstream == Token_Class.MultiplyOp)
                res = Token_Class.MultiplyOp;
            else
                return Token_Class.MinusOp;
            return res;
        }

        private Node Statement()
        {
            Node node = new Node("Statement");
            if (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.READ)
            {
                //READ
                node.Children.Add(match(Token_Class.READ));
                node.Children.Add(match(Token_Class.Idenifier));
                return node;
            }
            else if (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.WRITE)
            {
                //WRITE
                node.Children.Add(match(Token_Class.WRITE));
                if (TokenStream[InputPointer].token_type == Token_Class.ENDL)
                {
                    node.Children.Add(match(Token_Class.ENDL));
                    return node;
                }
                else
                {
                    node.Children.Add(expression());
                    return node;
                }
                return node;
            }
            else if (InputPointer < TokenStream.Count && (TokenStream[InputPointer].token_type == Token_Class.STRING || TokenStream[InputPointer].token_type == Token_Class.INT || TokenStream[InputPointer].token_type == Token_Class.FLOAT))
            {
                //Declaration Statemnet
                node.Children.Add(VarDecl());
            }
            else if (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.RETURN)
            {
                //RETURN
                node.Children.Add(match(Token_Class.RETURN));
                node.Children.Add(expression());
                return node;
            }
            else if (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.Idenifier && TokenStream[InputPointer + 1].token_type == Token_Class.Assign)
            {
                //ASSIGN
                node.Children.Add(identifiers());
                node.Children.Add(match(Token_Class.Assign));
                node.Children.Add(expression());
                return node;
            }
            else if (InputPointer < TokenStream.Count && TokenStream[InputPointer].token_type == Token_Class.Idenifier && TokenStream[InputPointer + 1].token_type == Token_Class.LParanthesis)
            {
                //Function Call
                node.Children.Add(function_call());
            }
            return node;
        }
        //mohamady






        public Node match(Token_Class ExpectedToken)
        {

            if (InputPointer < TokenStream.Count)
            {
                if (ExpectedToken == TokenStream[InputPointer].token_type)
                {
                    InputPointer++;
                    Node newNode = new Node(ExpectedToken.ToString());

                    return newNode;

                }

                else
                {
                    Errors.Error_List.Add("Parsing Error: Expected "
                        + ExpectedToken.ToString() + " and " +
                        TokenStream[InputPointer].token_type.ToString() +
                        "  found\r\n");
                    InputPointer++;
                    return null;
                }
            }
            else
            {
                Errors.Error_List.Add("Parsing Error: Expected "
                        + ExpectedToken.ToString()  + "\r\n");
                InputPointer++;
                return null;
            }
        }

        public static TreeNode PrintParseTree(Node root)
        {
            TreeNode tree = new TreeNode("Parse Tree");
            TreeNode treeRoot = PrintTree(root);
            if (treeRoot != null)
                tree.Nodes.Add(treeRoot);
            return tree;
        }
        static TreeNode PrintTree(Node root)
        {
            if (root == null || root.Name == null)
                return null;
            TreeNode tree = new TreeNode(root.Name);
            if (root.Children.Count == 0)
                return tree;
            foreach (Node child in root.Children)
            {
                if (child == null)
                    continue;
                tree.Nodes.Add(PrintTree(child));
            }
            return tree;
        }
    }
}
