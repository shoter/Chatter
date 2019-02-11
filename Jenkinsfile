pipeline
{
    agent any
    stages
    {       
        stage('Build')
        {
            steps
            {
                bat "\"${tool 'MSBuild'}\" Chatter.sln /t:Restore /p:Configuration=Release"
                bat "\"${tool 'MSBuild'}\" Chatter.sln /p:Configuration=Release"
            }
        }
        
        stage('Test')
        {
            steps
            {
                bat "dotnet test .\\Chatter.Core.Tests\\Chatter.Core.Tests.csproj"
            }
        }
        
        stage('Create Pull request')
        {
            steps
            {
                
            }
        }
    }
}