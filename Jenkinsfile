pipeline
{
    agent any
    stages
    {       
        stage('Build')
        {
            steps
            {
                script {
                bat "\"${tool '15.0 Visual Studio Community'}\" Chatter.sln /t:Restore /p:Configuration=Release"
                bat "\"${tool '15.0 Visual Studio Community'}\" Chatter.sln /p:Configuration=Release"
                } 
                
            }
        }
        
        stage('Test')
        {
            steps
            {
                bat "dotnet test .\\Chatter.Core.Tests\\Chatter.Core.Tests.csproj"
            }
        }
    }
}