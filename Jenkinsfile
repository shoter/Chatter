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
                bat "\"${tool 'MsBuild'}MsBuild.exe\" Chatter.sln /t:Restore /p:Configuration=Release"
                bat "\"${tool 'MsBuild'}MSBuild.exe\" Chatter.sln /p:Configuration=Release"
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